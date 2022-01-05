using Mirror;
using System.Collections;
using UnityEngine;

public class Archer : Attack
{
    //[SerializeField]
    //private GameObject projectile = null;

    //[SerializeField]
    //private Transform projectileSpawnPoint = null;

    //private float rotationSpeed = 20f;
    //private float lastFireTime;
    //private float m_Timer = 1;
    //private Unit m_Unit;

    //public override void OnStartServer()
    //{
    //    m_Unit = GetComponent<Unit>();
    //}



    //[ServerCallback]
    //private void Update()
    //{
    //    if (!(m_Unit.UnitMovement.Task == Task.Attack))
    //    {
    //        return;
    //    }

    //    StartCoroutine(UnitAttack());
    //    m_Timer -= Time.deltaTime;
    //}

    //private IEnumerator UnitAttack()
    //{
    //    yield return new WaitUntil(() => m_Timer <= 0);
    //    m_Timer = Stats.GetAttributeAmount(AttributeType.RateOfFire);

    //    var target = Targeter.Target;

    //    if (target == null)
    //    {
    //        yield break;
    //    }

    //    if (!IsCloseEnoughToTarget())
    //    {
    //        yield break;
    //    }

    //    var targetRotation =
    //        Quaternion.LookRotation(target.transform.position - transform.position);

    //    transform.rotation = Quaternion.RotateTowards(
    //        transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


    //    var projectileRotation = Quaternion.LookRotation(
    //        target.transform.position - projectileSpawnPoint.position);

    //    var projectileInstance = Instantiate(projectile, projectileSpawnPoint.position, projectileRotation);

    //    projectile.GetComponent<UnitProjectile>().DamageToDeal = Stats.GetAttributeAmount(AttributeType.Attack);
    //    projectile.GetComponent<UnitProjectile>().Sender = gameObject;

    //    NetworkServer.Spawn(projectileInstance, connectionToClient);

    //    lastFireTime = Time.time;

    //}

    //[Server]
    //public override bool IsCloseEnoughToTarget()
    //{
    //    if (Stats == null)
    //    {
    //        return false;
    //    }

    //    var unitRange = Stats.GetAttributeAmount(AttributeType.Range);
    //    var targetSize = Utils.GameObjectSize(Targeter.Target.Size);

    //    return (Targeter.Target.transform.position - transform.position).sqrMagnitude <=
    //         (unitRange + targetSize) * (unitRange + targetSize);
    //}
}
