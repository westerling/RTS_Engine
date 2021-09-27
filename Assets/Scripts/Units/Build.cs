using Mirror;
using UnityEngine;

public class Build : NetworkBehaviour
{
    private float rotationSpeed = 20f;
    private Builder builder;

    private void Start()
    {
        builder = GetComponent<Builder>();
    }

    [ServerCallback]
    private void Update()
    {
        var target = builder.Target;

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
                transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }       
    }

    [Server]
    private bool CanRepairTarget()
    {
        var size = Utils.AtBuildingEdge(builder.Target);

        return (builder.Target.transform.position - transform.position).sqrMagnitude <=
            (size) * (size);
    }

    [ClientRpc]
    private void RpcAddBuilder(Building building, Unit unit)
    {
        building.AddBuilder(unit);
    }

}
