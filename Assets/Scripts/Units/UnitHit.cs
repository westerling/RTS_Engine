using Mirror;
using UnityEngine;

public class UnitHit : NetworkBehaviour
{
    [SerializeField]
    private Targeter targeter = null;

    [SerializeField]
    private float attackRange = 5f;

    [SerializeField]
    private float attackRate = 1f;

    [SerializeField]
    private float rotationSpeed = 20f;

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

        var targetRotation =
            Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Time.time > (1 / attackRate) + lastFireTime)
        {
            //Attack

            lastFireTime = Time.time;
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        return (targeter.Target.transform.position - transform.position).sqrMagnitude <=
            attackRange * attackRange;
    }
}
