using Mirror;
using UnityEngine;

public class Melee : Attack
{    
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
            ClientDebug("not close enough");
            return;
        }
        ClientDebug("close enough");
        var targetRotation =
            Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Time.time > (1 / Stats.GetAttributeAmount(AttributeType.RateOfFire)) + lastFireTime)
        {
            if (target.TryGetComponent(out Health health))
            {
                health.DealDamage((int)Stats.GetAttributeAmount(AttributeType.Attack), (int)AttackStyle.Melee);
            }

            lastFireTime = Time.time;
        }
    }

    [Server]
    public override bool IsCloseEnoughToTarget()
    {
        if (Stats == null)
        {
            return false;
        }

        return (Targeter.Target.transform.position - transform.position).sqrMagnitude <= 
            Utils.DistanceToBuilding(Targeter.Target.Size) * Utils.DistanceToBuilding(Targeter.Target.Size);
    }

    [ClientRpc]
    private void ClientDebug(string message)
    {
        Debug.Log(message);
    }
}
