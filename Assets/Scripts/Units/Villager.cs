using Mirror;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class Villager : Unit, IBuilder, ICollector, IDeliver, IGarrison, IAttack
{
    [SerializeField]
    private CreateEntity[] m_Buildings;

    private float m_Timer = 1;
    private int m_CarryingAmount;

    private Resource m_CurrentResource;

    public event Action<int> ResourceCollected;

    public CreateEntity[] Buildings
    {
        get => m_Buildings;
        set => m_Buildings = value;
    }

    public Resource CurrentResource
    {
        get => m_CurrentResource; 
        set => m_CurrentResource = value;
    }
    
    public int CarryingAmount 
    { 
        get => m_CarryingAmount;
        set
        {
            m_CarryingAmount = value;
            ResourceCollected?.Invoke(value);
        }
    }

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
            case Task.Idle:
            case Task.Move:
                StopAllCoroutines();
                break;
        }
        
        m_Timer -= Time.deltaTime;
    }

    #region tasks

    [Server]
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

    [Server]
    public IEnumerator Collect()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = 1;

        var target = Targeter.Target;

        if (target == null)
        {
            yield break;
        }

        if (target.TryGetComponent(out IResource resource))
        {
            if (CarryingAmountIsFull())
            {
                UnitMovement.Deliver();
                yield break;
            }

            if (!Utils.IsCloseEnough(target, transform.position))
            {
                yield break;
            }

            if (!resource.CanGather)
            {
                FindNewResource(resource.Resource);
                yield break;
            }

            var resourceType = resource.Resource;

            var collectPerSecond = 1f;

            switch (resourceType)
            {
                case Resource.Food:
                    if (TryGetComponent(out IFoodResource foodResource))
                    {
                        switch (foodResource.FoodType)
                        {
                            case FoodType.Farm:
                                collectPerSecond = LocalStats.Stats.GetAttributeAmount(AttributeType.Farmer);
                                break;
                            case FoodType.Forage:
                                collectPerSecond = LocalStats.Stats.GetAttributeAmount(AttributeType.Forager);
                                break;
                            case FoodType.Wild:
                                collectPerSecond = LocalStats.Stats.GetAttributeAmount(AttributeType.Hunter);
                                break;
                            case FoodType.Domested:
                                collectPerSecond = LocalStats.Stats.GetAttributeAmount(AttributeType.Sheppard);
                                break;
                            default:
                                break;
                        }
                    }
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

            AddResource(resourceType);

            resource.GatherResources(1);

            var targetRotation =
                Quaternion.LookRotation(target.transform.position - transform.position);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRotation, 1f * Time.deltaTime);

            if (target.TryGetComponent(out Interactable interactable))
            {
                interactable.RpcStartHitParticles(transform);
            }
        }
    }

    [Server]
    public IEnumerator Deliver()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = 1;

        var interactable = Targeter.DropOff;

        if (interactable == null)
        {
            yield break;
        }

        if (interactable.TryGetComponent(out IDropOff dropOff))
        {

            if (!Utils.IsCloseEnough(interactable, transform.position))
            {
                yield break;
            }

            dropOff.Deliver(CarryingAmount);
            ResetCollector();
        }
    }

    [Server]
    public IEnumerator Build()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = 1;

        var target = Targeter.Target;

        if (target == null)
        {
            FindNewBuilding();
            yield break;
        }

        if (target.TryGetComponent(out Building building))
        {
            if (!Utils.IsCloseEnough(building, transform.position))
            {
                ClientDebug("too long");
                yield break;
            }

            var unit = GetComponent<Unit>();

            if (!building.HasBuilder(unit))
            {
                RpcAddBuilder(building, unit);
            }

            if (building.BuildingIsCompleted)
            {
                var cost = building.GetCostForRepairing();

                if (Utils.CanAfford(Player.GetResources(), cost))
                {
                    foreach (var resource in cost)
                    {
                        Player.CmdSetResources((int)resource.Key, -resource.Value);
                    }
                }
            }

            RotateTowardsTarget(target.transform.position);

            var amount = building.GetRepairAmountPerBuilder();

            if (target.TryGetComponent(out Health health))
            {
                var newHealth = health.CurrentHealth + amount;
                health.SetHealth(newHealth);

                if (health.HasFullHealth())
                {
                    FindNewBuilding();
                }
            }
        }
    }

    [Server]
    public IEnumerator Attack()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = LocalStats.Stats.GetAttributeAmount(AttributeType.RateOfFire);

        var target = Targeter.Target;

        if (target == null)
        {
            FindNewEnemy();
            yield break;
        }

        if (!Utils.IsCloseEnough(target, transform.position, LocalStats.Stats.GetAttributeAmount(AttributeType.Range)))
        {
            yield break;
        }

        RotateTowardsTarget(target.transform.position);

        if (target.TryGetComponent(out Health health))
        {
            health.DealDamage((int)LocalStats.Stats.GetAttributeAmount(AttributeType.Attack), (int)AttackStyle.Melee);

            if (health.CurrentHealth <= 0)
            {
                if (target.TryGetComponent(out IResource resource))
                {
                    UnitMovement.Collect();
                    Targeter.SetTarget(target.gameObject);
                }
            }

            if (target.TryGetComponent(out InteractableGameEntity targetable))
            {
                targetable.Reaction(gameObject);
            }
        }
    }

    #endregion

    public void ResetCollector()
    {
        CarryingAmount = 0;
        UnitMovement.Collect();
    }

    public void FindNewResource(Resource currentResource)
    {
        if (Targeter.FindNewTarget(Task.Collect, currentResource))
        {
            UnitMovement.Stop();
            return;
        }

        UnitMovement.Collect();
    }

    public void FindNewBuilding()
    {
        if (Targeter.FindNewTarget(Task.Build))
        {
            UnitMovement.Stop();
            return;
        }

        UnitMovement.Build();
    }

    public void FindNewEnemy()
    {
        if (Targeter.FindNewTarget(Task.Attack, LocalStats.Stats.GetAttributeAmount(AttributeType.LineOfSight)))
        {
            UnitMovement.Stop();
            return;
        }

        UnitMovement.Attack();
    }

    public override void AuthorityHandleUpgradeAdded(Upgrade upgrade)
    {
        base.AuthorityHandleUpgradeAdded(upgrade);
    }


    #region Client
    [ClientRpc]
    public void RpcAddBuilder(Building building, Unit unit)
    {
        building.AddBuilder(unit);
    }

    public override void AddBehaviours()
    {
        base.AddBehaviours();
        AddSwitchPanelsAction();
    }

    public bool CarryingAmountIsFull()
    {
        return CarryingAmount >= (int)LocalStats.Stats.GetAttributeAmount(AttributeType.CarryCapacity);
    }

    public void AddResource(Resource resource)
    {
        if (resource == CurrentResource)
        {
            CarryingAmount++;
        }
        else
        {
            CarryingAmount = 1;
        }

        CurrentResource = resource;
    }

    public void AddSwitchPanelsAction()
    {
        if (Buildings.Where(x => x.Position > 14).Any())
        {
            ActionBehaviours.Add(new SwitchPanelsAction(14));
            ActionBehaviours.Add(new SwitchPanelsAction(29));
        }
    }

    #endregion
}
