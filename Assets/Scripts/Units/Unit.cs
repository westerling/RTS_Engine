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

    #region server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);

        m_Health.ServerOnDie += ServerHandleDie;
        Upgrade.ServerOnUpgradeAdded += ServerHandleUpgradeAdded;
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);

        m_Health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region client

    public override void OnStartAuthority()
    {
        AuthorityOnUnitSpawned?.Invoke(this);
        Upgrade.AuthorityOnUpgradeAdded += AuthorityHandleUpgradeAdded;
    }

    private void AuthorityHandleUpgradeAdded(Upgrade upgrade)
    {
        //GetComponent<LocalStats>().AlterStats(upgrade.Stats);
    }

    private void ServerHandleUpgradeAdded(Upgrade obj)
    {
        //GetComponent<LocalStats>();
    }

    public override void OnStopClient()
    {
        if (!hasAuthority)
        {
            return;
        }

        AuthorityOnUnitDespawned?.Invoke(this);
    }

    public override void EnemyReaction(GameObject sender)
    {
        switch(Stance)
        {
            case StanceType.Offensive:
            case StanceType.Defensive:
                break;
            case StanceType.Flee:
                break;
        }
    }

    #endregion
}
