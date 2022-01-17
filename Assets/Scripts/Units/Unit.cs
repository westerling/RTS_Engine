using UnityEngine;
using System;
using Mirror;

public class Unit : InteractableGameEntity
{
    [SerializeField]
    private UnitMovement m_UnitMovement = null;

    private float m_RotationSpeed = 1f;

    private StanceType m_Stance = StanceType.Defensive;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;  
    
    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    public UnitMovement UnitMovement
    {
        get { return m_UnitMovement; }
    }

    public StanceType Stance
    {
        get { return m_Stance; }
        set { m_Stance = value; }
    }

    #region server

    public override void OnStartServer()
    {
        if (!hasAuthority)
        {
            return;
        }
            base.OnStartServer();

        ServerOnUnitSpawned?.Invoke(this);
        Upgrade.ServerOnUpgradeAdded += ServerHandleUpgradeAdded;
        SetFOVAvailability(true);
        
    }

    #endregion

    #region client

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        AuthorityOnUnitSpawned?.Invoke(this);
        SetFOVAvailability(true);
        Upgrade.AuthorityOnUpgradeAdded += AuthorityHandleUpgradeAdded;
    }

    public override void AddBehaviours()
    {
        AddKillEntityAction();
    }

    public virtual void AuthorityHandleUpgradeAdded(Upgrade upgrade)
    {

    }

    public virtual void ServerHandleUpgradeAdded(Upgrade obj)
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

    protected void RotateTowardsTarget(Vector3 target)
    {
        var targetRotation =
            Quaternion.LookRotation(target - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
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

    public override void ServerHandleDie()
    {
        ServerOnUnitDespawned?.Invoke(this);
        SetFOVAvailability(false);

        DestroyThisOnServer();
    }

    #endregion
}
