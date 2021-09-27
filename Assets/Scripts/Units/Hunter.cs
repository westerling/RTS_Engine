using Mirror;
using UnityEngine;

public class Hunter : NetworkBehaviour
{
    [SerializeField]
    private Targeter targeter = null;

    [SerializeField]
    private GameObject projectile = null;

    [SerializeField]
    private Transform projectileSpawnPoint = null;

    [SerializeField]
    private float rotationSpeed = 20f;

    private Stats m_Stats;
    private float lastFireTime;

    [ServerCallback]
    private void Update()
    {
        var target = targeter.Target;

        if (target == null)
        {
            return;
        }

        if (!CanFireAtTarget())
        {
            return;
        }

        //var stats = statsManager.GetUnitStats(GetComponent<Unit>().Id);
        m_Stats = GetComponent<LocalStats>().Stats;

        var targetRotation =
            Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Time.time > (1 / m_Stats.GetAttributeAmount(AttributeType.RateOfFire)) + lastFireTime)
        {
            var projectileRotation = Quaternion.LookRotation(
                target.transform.position - projectileSpawnPoint.position);

            var projectileInstance = Instantiate(projectile, projectileSpawnPoint.position, projectileRotation);

            NetworkServer.Spawn(projectileInstance, connectionToClient);

            lastFireTime = Time.time;
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        if (m_Stats == null)
        {
            return false;
        }

        return (targeter.Target.transform.position - transform.position).sqrMagnitude <=
            (m_Stats.GetAttributeAmount(AttributeType.Range) * m_Stats.GetAttributeAmount(AttributeType.Range));
    }
}
