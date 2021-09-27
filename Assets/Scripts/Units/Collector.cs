using Mirror;
using System;
using UnityEngine;

public class Collector : BaseUnitClickAction
{
    public event Action<int> ResourceCollected;

    [SyncVar]
    private Collectable m_Target;

    [SyncVar]
    private GameObject m_DeliveryPoint;
    
    [SyncVar]
    private int m_CarryingAmount;
    
    [SyncVar]
    private Resource m_Resource;
    
    [SyncVar]
    private int m_CarryCapacity = 10;

    public Collectable Target
    {
        get { return m_Target; }
        set { m_Target = value; }
    }

    public GameObject DeliveryPoint
    {
        get { return m_DeliveryPoint; }
        set { m_DeliveryPoint = value; }
    }

    public Resource Resource
    {
        get { return m_Resource; }
        set { m_Resource = value; }
    }

    public int CarryingAmount
    {
        get { return m_CarryingAmount; }
        set 
        {
            m_CarryingAmount = value;
            ResourceCollected?.Invoke(value);
        }
    }

    public int CarryingCapacity
    {
        get { return m_CarryCapacity; }
        set { m_CarryCapacity = value; }
    }

    public bool CarryingAmountIsFull()
    {
        return CarryingAmount >= CarryingCapacity;
    }


    public void AddResource(Resource newResource)
    {
        if (newResource == m_Resource)
        {
            CarryingAmount++;
        }
        else
        {
            CarryingAmount = 1;
        }

        Resource = newResource;
    }

    #region server

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent(out Collectable newTarget))
        {
            return;
        }

        if (newTarget.CurrentGatherers >= newTarget.MaxGatherers)
        {
            return;
        }
        
        Target = newTarget;
    }

    [Command]
    public void CmdSetDropOff(GameObject dropOffGameObject)
    {
        if (dropOffGameObject.TryGetComponent(out DropOff dropOff))
        {
            DeliveryPoint = dropOff.gameObject;
            return;
        }
        else if (dropOffGameObject.TryGetComponent(out TownCenter townCenter))
        {
            DeliveryPoint = townCenter.gameObject;
            return;
        }
    }

    [Command]
    public void CmdClearTarget()
    {
        ClearTarget();
    }

    [Server]
    public override void ClearTarget()
    {
        Target = null;
        DeliveryPoint = null;
    }

    #endregion

    public override void UpdateStats()
    {
        var stats = GetComponent<LocalStats>().Stats;

        CarryingCapacity = (int)stats.GetAttributeAmount(AttributeType.CarryCapacity);
    }
}
