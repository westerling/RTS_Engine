using Mirror;
using UnityEngine;

public class Archer : Attack
{

    [SerializeField]
    private GameObject projectile = null;

    [SerializeField]
    private Transform projectileSpawnPoint = null;

    private float rotationSpeed = 20f;
    private float lastFireTime;


    [ServerCallback]
    private void Update()
    {
        var target = Targeter.Target;

        if (target == null)
        {
            return;
        }

        if (!IsCloseEnoughToTarget())
        {
            return;
        }

        var targetRotation = 
            Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Time.time > (1 / Stats.GetAttributeAmount(AttributeType.RateOfFire)) + lastFireTime)
        {
            var projectileRotation = Quaternion.LookRotation(
                target.transform.position - projectileSpawnPoint.position);

            var projectileInstance = Instantiate(projectile, projectileSpawnPoint.position, projectileRotation);

            projectile.GetComponent<UnitProjectile>().DamageToDeal = Stats.GetAttributeAmount(AttributeType.Attack);

            NetworkServer.Spawn(projectileInstance, connectionToClient);

            lastFireTime = Time.time;
        }
    }
}
