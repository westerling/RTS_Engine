using Mirror;
using System.Collections;
using UnityEngine;

public class Swordman : Unit, IGarrison, IAttack
{
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
            ClientDebug("Target null");
            yield break;
        }

        if (!Utils.IsCloseEnough(target, transform.position, LocalStats.Stats.GetAttributeAmount(AttributeType.Range)))
        {
            ClientDebug("Too far");
            yield break;
        }

        RotateTowardsTarget(target.transform.position);

        if (target.TryGetComponent(out Health health))
        {
            ClientDebug("Attack: " + LocalStats.Stats.GetAttributeAmount(AttributeType.Attack));
            health.DealDamage((int)LocalStats.Stats.GetAttributeAmount(AttributeType.Attack), (int)AttackStyle.Melee);

            if (target.TryGetComponent(out InteractableGameEntity targetable))
            {
                targetable.Reaction(gameObject);
            }
        }
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
