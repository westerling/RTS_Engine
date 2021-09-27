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

            if (!CanDeliver(deliveryPoint.transform))
            {
                return;
            }

            dropOff.Deliver(collector.CarryingAmount);
            ResetCollector();
        }
        if (deliveryPoint.TryGetComponent(out TownCenter townCenter))
        {

            if (!CanDeliver(deliveryPoint.transform))
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
    private bool CanDeliver(Transform transform)
    {
        return (transform.position - this.transform.position).sqrMagnitude <=
            (4f) * (4f);
    }
}

