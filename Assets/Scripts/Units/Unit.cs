using UnityEngine;
using Mirror;
using System;

public class Unit : Targetable
{
    [SerializeField]
    private Health m_Health = null;

    [SerializeField]
    private UnitMovement m_UnitMovement = null;

    [SerializeField]
    private Collector m_Collector = null;

    [SerializeField]
    private Builder m_Builder = null;

    [SerializeField]
    private UnitRally m_UnitRally = null;

    private Building m_GarrisonBuilding = null;

    private StanceType m_Stance = StanceType.Defensive;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;  
    
    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    public UnitMovement UnitMovement
    {
        get { return m_UnitMovement; }
    }

    public Collector Collector
    {
        get { return m_Collector; }
    }

    public Builder Builder
    {
        get { return m_Builder; }
    }

    public StanceType Stance
    {
        get { return m_Stance; }
        set { m_Stance = value; }
    }

    public Building GarrisonBuilding
    { 
        get => m_GarrisonBuilding; 
        set => m_GarrisonBuilding = value; 
    }

    #region server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
        m_Health.ServerOnDie += ServerHandleDie;
        Upgrade.ServerOnUpgradeAdded += ServerHandleUpgradeAdded;

        if (hasAuthority)
        {
            SetFOVAvailability(true);
        }
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
        SetFOVAvailability(false);
        m_Health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    public void CmdSetGarrison(Building garrison)
    {
        GarrisonBuilding = garrison;
    }

    [Command]
    public void CmdClearGarrison()
    {
        ClearGarrison();
    }

    [Server]
    public void ClearGarrison()
    {
        if (GarrisonBuilding == null)
        {
            return;
        }

        GarrisonBuilding = null;
    }
    #endregion

    #region client

    public override void OnStartAuthority()
    {
        AuthorityOnUnitSpawned?.Invoke(this);
        SetFOVAvailability(true);
        Upgrade.AuthorityOnUpgradeAdded += AuthorityHandleUpgradeAdded;
    }

    private void AuthorityHandleUpgradeAdded(Upgrade upgrade)
    {

    }

    private void ServerHandleUpgradeAdded(Upgrade obj)
    {

    }

    public override void OnStopClient()
    {
        if (!hasAuthority)
        {
            return;
        }

        SetFOVAvailability(false);
        AuthorityOnUnitDespawned?.Invoke(this);
    }

    public override void Reaction(GameObject sender)
    {
        switch(Stance)
        {
            case StanceType.Offensive:
                TryTarget(sender);
                break;
            case StanceType.Defensive:
                break;
            case StanceType.Flee:
                TryFlee();
                break;
        }
    }

    private void TryFlee()
    {
        throw new NotImplementedException();
    }

    private void TryTarget(GameObject target)
    {
        if (TryGetComponent(out Targeter targeter))
        {
            if (targeter == null)
            {
                return;
            }

            targeter.CmdSetTarget(target);

            UnitMovement.SetTask((int)Task.Attack);
            UnitMovement.Attack();
        }
    }

    #endregion
}
