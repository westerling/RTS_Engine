using Mirror;
using System.Collections;
using UnityEngine;

public class Build : NetworkBehaviour
{
    private float m_RotationSpeed = 20f;
    private Builder m_Builder;
    private Unit m_Unit;
    private float m_Timer = 1;

    public override void OnStartServer()
    {
        m_Unit = GetComponent<Unit>();
        m_Builder = GetComponent<Builder>();
    }

    [ServerCallback]
    private void Update()
    {
        if (!(m_Unit.UnitMovement.Task == Task.Build))
        {
            return;
        }

        StartCoroutine(UnitBuild());
        m_Timer -= Time.deltaTime;
    }

    private IEnumerator UnitBuild()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = 1;
        
        var target = m_Builder.Target;

        if (target == null)
        {
            SetNewTarget();
            yield break;
        }

        if (target.TryGetComponent(out Building building))
        {
            if (!IsCloseEnough())
            {
                yield break;
            }

            var unit = GetComponent<Unit>();

            if (!building.HasBuilder(unit))
            {
                RpcAddBuilder(building, unit);
            }

            if (building.BuildingIsCompleted)
            {
                var player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
                var cost = building.GetCostForRepairing();

                if (Utils.CanAfford(player.GetResources(), cost))
                {
                    foreach (var resource in cost)
                    {
                        player.CmdSetResources((int)resource.Key, -resource.Value);
                    }
                }
            }

            RotateTowardsTarget(target);

            var amount = target.GetRepairAmountPerBuilder();

            if (target.TryGetComponent(out Health health))
            {
                var newHealth = health.CurrentHealth + amount;
                health.SetHealth(newHealth);

                if (health.HasFullHealth())
                {
                    SetNewTarget();
                }
            }
        }
    }

    private void SetNewTarget()
    {
        m_Builder.FindNewTarget();
        ResetBuilder();
    }

    private void RotateTowardsTarget(Building target)
    {
        var targetRotation = 
            Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
    }

    [Server]
    private bool IsCloseEnough()
    {
        var size = Utils.GameObjectSize(m_Builder.Target.Size);

        return (m_Builder.Target.transform.position - transform.position).sqrMagnitude <=
            (size) * (size);
    }

    [ClientRpc]
    private void RpcAddBuilder(Building building, Unit unit)
    {
        building.AddBuilder(unit);
    }

    private void ResetBuilder()
    {
        m_Unit.UnitMovement.Build();
    }

    [ClientRpc]
    private void ClientDebug(string message)
    {
        Debug.Log(message);
    }
}
