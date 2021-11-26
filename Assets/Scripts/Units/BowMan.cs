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
    private float m_RotationSpeed = 20f;

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

    public IEnumerator Attack()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = LocalStats.Stats.GetAttributeAmount(AttributeType.RateOfFire);

        var target = Targeter.Target;

        if (target == null)
        {
            yield break;
        }

        if (!(Utils.IsCloseEnough(target,transform.position, LocalStats.Stats.GetAttributeAmount(AttributeType.Range))))
        {
            yield break;
        }

        RotateTowardsTarget(target.transform.position);

        var projectileRotation = Quaternion.LookRotation(
            target.transform.position - m_ProjectileSpawnPoint.position);

        var projectileInstance = Instantiate(m_Projectile, m_ProjectileSpawnPoint.position, projectileRotation);

        m_Projectile.GetComponent<UnitProjectile>().DamageToDeal = LocalStats.Stats.GetAttributeAmount(AttributeType.Attack);
        m_Projectile.GetComponent<UnitProjectile>().Sender = gameObject;

        NetworkServer.Spawn(projectileInstance, connectionToClient);
    }

    #endregion

    private void RotateTowardsTarget(Vector3 target)
    {
        var targetRotation =
            Quaternion.LookRotation(target - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
    }
}
