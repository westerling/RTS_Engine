using Mirror;
using System.Collections;
using UnityEngine;

public class BowMan : Unit, IGarrison, IAttack
{
    [SerializeField]
    private GameObject m_Projectile = null;

    [SerializeField]
    private Transform m_ProjectileSpawnPoint = null;

    private float m_Timer = 1;

    [ServerCallback]
    private void Update()
    {

        switch (UnitMovement.Task)
        {
            case Task.Attack:
                StartCoroutine(Attack());
                break;
            case Task.Garrison:
                StartCoroutine(Garrison());
                break;
            case Task.Idle:
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
    public IEnumerator Attack()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = LocalStats.Stats.GetAttributeAmount(AttributeType.RateOfFire);

        var target = Targeter.Target;

        if (target == null)
        {
            FindNewEnemy();
            ClientDebug("Target null");
            yield break;
        }

        if (!(Utils.IsCloseEnough(target,transform.position, LocalStats.Stats.GetAttributeAmount(AttributeType.Range))))
        {
            ClientDebug("Too far");
            yield break;
        }

        RotateTowardsTarget(target.transform.position);

        var projectileRotation = Quaternion.LookRotation(
            target.transform.position - m_ProjectileSpawnPoint.position);

        var projectileInstance = Instantiate(m_Projectile, m_ProjectileSpawnPoint.position, projectileRotation);

        var unitProjectile = m_Projectile.GetComponent<UnitProjectile>();

        unitProjectile.DamageToDeal = LocalStats.Stats.GetAttributeAmount(AttributeType.Attack);
        unitProjectile.Sender = gameObject;

        NetworkServer.Spawn(projectileInstance, connectionToClient);

        ClientDebug("Shoot the fucker");
    }

    #endregion

    public void FindNewEnemy()
    {
        if (Targeter.FindNewTarget(Task.Attack, LocalStats.Stats.GetAttributeAmount(AttributeType.LineOfSight)))
        {
            UnitMovement.Stop();
            return;
        }

        UnitMovement.Attack();
    }
}
