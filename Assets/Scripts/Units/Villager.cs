using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : Unit, IBuild, ICollect, IDeliver, IGarrison, IAttack
{
    private float m_Timer = 1;
    private float m_RotationSpeed = 1f;

    [ServerCallback]
    private void Update()
    {

        switch (UnitMovement.Task)
        {
            case Task.Attack:
                StartCoroutine(Attack());
                break;
            case Task.Build:
                StartCoroutine(Build());
                break;
            case Task.Collect:
                StartCoroutine(Collect());
                break;
            case Task.Deliver:
                StartCoroutine(Deliver());
                break;
            case Task.Garrison:
                StartCoroutine(Garrison());
                break;
        }
        
        m_Timer -= Time.deltaTime;
    }

    #region tasks

    public IEnumerator Garrison()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = 1;

        var target = Targeter.Target;

        if (target.TryGetComponent(out Building building))
        {
            if (Utils.IsCloseEnough(target, transform.position))
            {
                if (building.CanGarrisonUnits())
                {
                    building.GatherUnit(this);
                }
            }
        }
    }

    public IEnumerator Collect()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = 1;

        var target = Collector.Target;

        if (target == null)
        {
            yield break;
        }

        if (target.TryGetComponent(out Collectable collectable))
        {
            if (Collector.CarryingAmountIsFull())
            {
                UnitMovement.Deliver();
                yield break;
            }

            if (!Utils.IsCloseEnough(target, transform.position))
            {
                yield break;
            }

            if (!collectable.CanGather())
            {
                FindNewResource(collectable.Resource);
                yield break;
            }

            var resourceType = target.Resource;

            var collectPerSecond = 1f;

            switch (resourceType)
            {
                case Resource.Food:
                    collectPerSecond = LocalStats.Stats.GetAttributeAmount(AttributeType.Farmer);
                    break;
                case Resource.Gold:
                    collectPerSecond = LocalStats.Stats.GetAttributeAmount(AttributeType.GoldMiner);
                    break;
                case Resource.Stone:
                    collectPerSecond = LocalStats.Stats.GetAttributeAmount(AttributeType.StoneMiner);
                    break;
                case Resource.Wood:
                    collectPerSecond = LocalStats.Stats.GetAttributeAmount(AttributeType.Lumberjack);
                    break;
            }

            m_Timer = 1f / collectPerSecond;

            Collector.AddResource(resourceType);

            collectable.GatherResources(1);

            var targetRotation =
                Quaternion.LookRotation(target.transform.position - transform.position);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
        }
    }

    public IEnumerator Deliver()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = 1;

        var deliveryPoint = Collector.DeliveryPoint;

        if (deliveryPoint == null)
        {
            yield break;
        }

        if (deliveryPoint.TryGetComponent(out DropOff dropOff))
        {

            if (!Utils.IsCloseEnough(dropOff, transform.position))
            {
                yield break;
            }

            dropOff.Deliver(Collector.CarryingAmount);
            ResetCollector();
        }
        if (deliveryPoint.TryGetComponent(out TownCenter townCenter))
        {

            if (!Utils.IsCloseEnough(townCenter, transform.position))
            {
                yield break;
            }

            townCenter.Deliver(Collector.Resource, Collector.CarryingAmount);
            ResetCollector();
        }
    }

    public IEnumerator Build()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = 1;

        var target = Builder.Target;

        if (target == null)
        {
            SetNewTarget();
            yield break;
        }

        if (target.TryGetComponent(out Building building))
        {
            if (!Utils.IsCloseEnough(building, transform.position))
            {
                yield break;
            }

            var unit = GetComponent<Unit>();

            if (!building.HasBuilder(unit))
            {
                RpcAddBuilder(building, unit);
            }

            if (building.BuildingIsCompleted)
            {
                var player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
                var cost = building.GetCostForRepairing();

                if (Utils.CanAfford(player.GetResources(), cost))
                {
                    foreach (var resource in cost)
                    {
                        player.CmdSetResources((int)resource.Key, -resource.Value);
                    }
                }
            }

            RotateTowardsTarget(target.transform.position);

            var amount = target.GetRepairAmountPerBuilder();

            if (target.TryGetComponent(out Health health))
            {
                var newHealth = health.CurrentHealth + amount;
                health.SetHealth(newHealth);

                if (health.HasFullHealth())
                {
                    SetNewTarget();
                }
            }
        }
    }

    public IEnumerator Attack()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = LocalStats.Stats.GetAttributeAmount(AttributeType.RateOfFire);

        var target = Targeter.Target;

        if (target == null)
        {
            yield break;
        }

        if (!Utils.IsCloseEnough(target, transform.position, LocalStats.Stats.GetAttributeAmount(AttributeType.Range)))
        {
            ClientDebug("not close enough");
            yield break;
        }

        RotateTowardsTarget(target.transform.position);

        if (target.TryGetComponent(out Health health))
        {
            health.DealDamage((int)LocalStats.Stats.GetAttributeAmount(AttributeType.Attack), (int)AttackStyle.Melee);

            if (target.TryGetComponent(out Targetable targetable))
            {
                ClientDebug("Fuck yeah");
                targetable.Reaction(gameObject);
            }
        }
    }

    #endregion

    public void ResetCollector()
    {
        Collector.CarryingAmount = 0;
        UnitMovement.Collect();
    }

    public void FindNewResource(Resource currentResource)
    {
        var resourceArray = GameObject.FindGameObjectsWithTag("Resource");
        var resourceList = new List<GameObject>();

        foreach (var resource in resourceArray)
        {
            if (resource.GetComponent<Collectable>().Resource == currentResource)
            {
                resourceList.Add(resource);
            }
        }

        resourceList.Sort((go1, go2) => Vector3.Distance(transform.position, go1.transform.position).CompareTo(Vector3.Distance(transform.position, go2.transform.position)));
    }

    public void SetNewTarget()
    {
        Builder.FindNewTarget();
        ResetBuilder();
    }

    private void RotateTowardsTarget(Vector3 target)
    {
        var targetRotation =
            Quaternion.LookRotation(target - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
    }

    public void ResetBuilder()
    {
        UnitMovement.Build();
    }

    #region Client
    [ClientRpc]
    public void RpcAddBuilder(Building building, Unit unit)
    {
        building.AddBuilder(unit);
    }
    #endregion
}
