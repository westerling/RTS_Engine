using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : Unit, IBuild, ICollect, IDeliver
{
    private float m_Timer = 1;
    private float m_RotationSpeed = 20f;

    [ServerCallback]
    private void Update()
    {

        switch (UnitMovement.Task)
        {
            case Task.Attack:
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
        }
        
        m_Timer -= Time.deltaTime;
    }

    #region tasks

    public IEnumerator Collect()
    {
        var stats = GetLocalStats();

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

            if (!IsCloseEnough(target))
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
                    collectPerSecond = stats.GetAttributeAmount(AttributeType.Farmer);
                    break;
                case Resource.Gold:
                    collectPerSecond = stats.GetAttributeAmount(AttributeType.GoldMiner);
                    break;
                case Resource.Stone:
                    collectPerSecond = stats.GetAttributeAmount(AttributeType.StoneMiner);
                    break;
                case Resource.Wood:
                    collectPerSecond = stats.GetAttributeAmount(AttributeType.Lumberjack);
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

            if (!IsCloseEnough(dropOff))
            {
                yield break;
            }

            dropOff.Deliver(Collector.CarryingAmount);
            ResetCollector();
        }
        if (deliveryPoint.TryGetComponent(out TownCenter townCenter))
        {

            if (!IsCloseEnough(townCenter))
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
            if (!IsCloseEnough(building))
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

            RotateTowardsTarget(target);

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

    private void RotateTowardsTarget(Building target)
    {
        var targetRotation =
            Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
    }

    private bool IsCloseEnough(Interactable interactable)
    {
        var size = Utils.DistanceToBuilding(interactable.Size);

        return (Builder.Target.transform.position - transform.position).sqrMagnitude <=
            (size) * (size);
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
