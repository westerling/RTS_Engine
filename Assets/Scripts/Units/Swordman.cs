using Mirror;
using System.Collections;
using UnityEngine;

public class Swordman : Unit, IGarrison, IAttack
{
    private float m_Timer = 1;
    private float m_RotationSpeed = 100f;

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

        if (!Utils.IsCloseEnough(target, transform.position, LocalStats.Stats.GetAttributeAmount(AttributeType.Range)))
        {
            yield break;
        }

        RotateTowardsTarget(target.transform.position);

        if (target.TryGetComponent(out Health health))
        {
            health.DealDamage((int)LocalStats.Stats.GetAttributeAmount(AttributeType.Attack), (int)AttackStyle.Melee);

            if (target.TryGetComponent(out Targetable targetable))
            {
                targetable.Reaction(gameObject);
            }
        }
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
