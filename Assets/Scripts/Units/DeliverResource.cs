using Mirror;
using UnityEngine;

public class DeliverResource : NetworkBehaviour
{
    private Collector collector;
    private Unit unit;

    public override void OnStartServer()
    {
        unit = GetComponent<Unit>();
        collector = GetComponent<Collector>();
    }

    [ServerCallback]
    private void Update()
    {
        if (unit.UnitMovement?.Task == Task.Deliver)
        {
            ClientDebug("deliver!");
            Deliver();
        }
    }

    private void Deliver()
    {
        var deliveryPoint = collector.DeliveryPoint;

        if (deliveryPoint == null)
        {
            return;
        }

        if (deliveryPoint.TryGetComponent(out DropOff dropOff))
        {

            if (!CanDeliver(dropOff))
            {
                return;
            }

            dropOff.Deliver(collector.CarryingAmount);
            ResetCollector();
        }
        if (deliveryPoint.TryGetComponent(out TownCenter townCenter))
        {

            if (!CanDeliver(townCenter))
            {
                return;
            }

            townCenter.Deliver(collector.Resource, collector.CarryingAmount);
            ResetCollector();
        }
    }

    private void ResetCollector()
    {
        collector.CarryingAmount = 0;
        unit.UnitMovement.Collect();
    }

    [Server]
    private bool CanDeliver(Building building)
    {
        return (transform.position - this.transform.position).sqrMagnitude <=
            (Utils.DistanceToBuilding(building.Size)) * (Utils.DistanceToBuilding(building.Size));
    }

    private void ClientDebug(string text)
    {
        Debug.Log(text);
    }
}

