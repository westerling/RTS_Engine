using Mirror;
using System.Collections;
using UnityEngine;

public class UnitRally : NetworkBehaviour
{
    private Unit m_Unit;
    private float m_Timer = 1;

    public override void OnStartServer()
    {
        m_Unit = GetComponent<Unit>();
    }

    [ServerCallback]
    private void Update()
    {
        if (!(m_Unit.UnitMovement.Task == Task.Rally))
        {
            return;
        }

        StartCoroutine(Rally());
        m_Timer -= Time.deltaTime;
    }

    private IEnumerator Rally()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        m_Timer = 1;

        var target = m_Unit.GarrisonBuilding;

        if (IsCloseEnough(target))
        {
            if (target.CanGarrisonUnits())
            {
                target.GatherUnit(m_Unit);
            }
        }
    }

    [Server]
    private bool IsCloseEnough(Building building)
    {
        return (transform.position - transform.position).sqrMagnitude <=
            (Utils.DistanceToBuilding(building.Size)) * (Utils.DistanceToBuilding(building.Size));
    }
}