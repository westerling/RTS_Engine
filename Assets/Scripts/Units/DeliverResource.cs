using Mirror;
using System.Collections;
using UnityEngine;

public class DeliverResource : NetworkBehaviour
{
    //private Collector collector;
    //private Unit unit;
    //private float m_Timer = 1;

    //public override void OnStartServer()
    //{
    //    unit = GetComponent<Unit>();
    //    collector = GetComponent<Collector>();
    //}

    //[ServerCallback]
    //private void Update()
    //{
    //    if (!(unit.UnitMovement?.Task == Task.Deliver))
    //    {
    //        return;
    //    }

    //    StartCoroutine(Deliver());
    //    m_Timer -= Time.deltaTime;
    //}

    //private IEnumerator Deliver()
    //{
    //    yield return new WaitUntil(() => m_Timer <= 0);
    //    m_Timer = 1;

    //    var deliveryPoint = collector.DeliveryPoint;

    //    if (deliveryPoint == null)
    //    {
    //        yield break;
    //    }

    //    if (deliveryPoint.TryGetComponent(out DropOff dropOff))
    //    {

    //        if (!CanDeliver(dropOff))
    //        {
    //            yield break;
    //        }

    //        dropOff.Deliver(collector.CarryingAmount);
    //        ResetCollector();
    //    }
    //    if (deliveryPoint.TryGetComponent(out TownCenter townCenter))
    //    {

    //        if (!CanDeliver(townCenter))
    //        {
    //            yield break;
    //        }

    //        townCenter.Deliver(collector.Resource, collector.CarryingAmount);
    //        ResetCollector();
    //    }
    //}

    //private void ResetCollector()
    //{
    //    collector.CarryingAmount = 0;
    //    unit.UnitMovement.Collect();
    //}

    //[Server]
    //private bool CanDeliver(Building building)
    //{
    //    return (transform.position - this.transform.position).sqrMagnitude <=
    //        (Utils.GameObjectSize(building.Size)) * (Utils.GameObjectSize(building.Size));
    //}

    //private void ClientDebug(string text)
    //{
    //    Debug.Log(text);
    //}
}

