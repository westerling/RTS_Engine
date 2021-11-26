using Mirror;
using System.Collections;
using UnityEngine;

public class Melee : Attack
{    
    private float m_RotationSpeed = 5f;
    private float m_Timer = 1;
    private Unit m_Unit;

    public override void OnStartServer()
    {
        m_Unit = GetComponent<Unit>();
    }

    [ServerCallback]
    private void Update()
    {
        if (!(m_Unit.UnitMovement.Task == Task.Attack))
        {
            return;
        }

        StartCoroutine(UnitAttack());
        m_Timer -= Time.deltaTime;       
    }

    private IEnumerator UnitAttack()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = Stats.GetAttributeAmount(AttributeType.RateOfFire);

        var target = Targeter.Target;

        if (target == null)
        {
            ClientDebug("Target null");
            yield break;
        }

        if (!IsCloseEnoughToTarget())
        {
            ClientDebug("No");
            yield break;
        }
        ClientDebug("Yes");
        var targetRotation =
            Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);

        if (target.TryGetComponent(out Health health))
        {
            health.DealDamage((int)Stats.GetAttributeAmount(AttributeType.Attack), (int)AttackStyle.Melee);

            if (target.TryGetComponent(out Targetable targetable))
            {
                targetable.Reaction(gameObject);
            }
        }
    }

    [Server]
    public override bool IsCloseEnoughToTarget()
    {
        if (Stats == null)
        {
            return false;
        }

        var unitRange = Stats.GetAttributeAmount(AttributeType.Range);
        var targetSize = Utils.GameObjectSize(Targeter.Target.Size);

        return (Targeter.Target.transform.position - transform.position).sqrMagnitude <=
             (unitRange + targetSize) * (unitRange + targetSize);
    }

    [ClientRpc]
    private void ClientDebug(string message)
    {
        Debug.Log(message);
    }
}
