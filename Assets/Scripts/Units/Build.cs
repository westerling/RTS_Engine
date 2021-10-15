using Mirror;
using UnityEngine;

public class Build : NetworkBehaviour
{
    private float m_RotationSpeed = 20f;
    private Builder m_Builder;
    private Unit m_Unit;

    public override void OnStartServer()
    {
        m_Unit = GetComponent<Unit>();
        m_Builder = GetComponent<Builder>();
    }

    [ServerCallback]
    private void Update()
    {
        if (m_Unit.UnitMovement.Task == Task.Build)
        {
            UnitBuild();
            return;
        } 
    }

    private void UnitBuild()
    {
        var target = m_Builder.Target;

        if (target == null)
        {
            return;
        }

        if (target.TryGetComponent(out Building building))
        {
            if (!CanRepairTarget())
            {
                return;
            }

            var unit = GetComponent<Unit>();

            if (!building.HasBuilder(unit))
            {
                RpcAddBuilder(building, unit);
            }

            var targetRotation =
                Quaternion.LookRotation(target.transform.position - transform.position);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
        }
    }

    [Server]
    private bool CanRepairTarget()
    {
        var size = Utils.AtBuildingEdge(m_Builder.Target);

        return (m_Builder.Target.transform.position - transform.position).sqrMagnitude <=
            (size) * (size);
    }

    [ClientRpc]
    private void RpcAddBuilder(Building building, Unit unit)
    {
        building.AddBuilder(unit);
    }

    [ClientRpc]
    private void ClientDebug(string message)
    {
        Debug.Log(message);
    }
}
