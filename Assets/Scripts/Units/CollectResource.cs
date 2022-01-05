using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResource : NetworkBehaviour
{
    //private float rotationSpeed = 20f;
    //private Collector collector;
    //private Unit unit;
    //private float m_Timer = 1;
    //private float farmerSpeed = 1f;
    //private float forageSpeed = 1f;
    //private float goldMinerSpeed = 1f;
    //private float hunterSpeed = 1f;
    //private float lumberjackSpeed = 1f;
    //private float stoneMinerSpeed = 1f;

    //public override void OnStartServer()
    //{
    //    unit = GetComponent<Unit>();

    //    SetCollectingSpeed();

    //    collector = GetComponent<Collector>();
    //    GetComponent<LocalStats>().StatsAltered += HandleAlteredStats;
    //}

    //public override void OnStopServer()
    //{
    //    GetComponent<LocalStats>().StatsAltered -= HandleAlteredStats;
    //}

    //[ServerCallback]
    //private void Update()
    //{
    //    if (!(unit.UnitMovement.Task == Task.Collect))
    //    {
    //        return;
    //    }

    //    StartCoroutine(Collect());
    //    m_Timer -= Time.deltaTime;
    //}

    //private IEnumerator Collect()
    //{
    //    yield return new WaitUntil(() => m_Timer <= 0);
    //    m_Timer = 1;

    //    var target = collector.Target;

    //    if (target == null)
    //    {
    //        yield break;
    //    }

    //    if (target.TryGetComponent(out Collectable resource))
    //    {
    //        if (collector.CarryingAmountIsFull())
    //        {
    //            unit.UnitMovement.Deliver();
    //            yield break;
    //        }

    //        if (!CanCollectTarget(target.transform))
    //        {
    //            yield break;
    //        }

    //        if (!resource.CanGather())
    //        {
    //            FindNewResource();
    //            yield break;
    //        }

    //        var resourceType = target.Resource;

    //        var collectPerSecond = 1f;

    //        switch (resourceType)
    //        {
    //            case Resource.Food:
    //                collectPerSecond = farmerSpeed;
    //                break;
    //            case Resource.Gold:
    //                collectPerSecond = goldMinerSpeed;
    //                break;
    //            case Resource.Stone:
    //                collectPerSecond = stoneMinerSpeed;
    //                break;
    //            case Resource.Wood:
    //                collectPerSecond = lumberjackSpeed;
    //                break;
    //        }

    //        m_Timer = 1f / collectPerSecond;

    //        collector.AddResource(resourceType);

    //        resource.GatherResources(1);

    //        var targetRotation =
    //            Quaternion.LookRotation(target.transform.position - transform.position);

    //        transform.rotation = Quaternion.RotateTowards(
    //            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //    }
    //}

    //private void FindNewResource()
    //{
    //    var resourceArray = GameObject.FindGameObjectsWithTag("Resource");
    //    var resourceList = new List<GameObject>();

    //    foreach (var resource in resourceArray)
    //    {
    //        if (resource.GetComponent<Collectable>().Resource == collector.Resource)
    //        {
    //            resourceList.Add(resource);
    //        }
    //    }

    //    resourceList.Sort((go1, go2) => Vector3.Distance(transform.position, go1.transform.position).CompareTo(Vector3.Distance(transform.position, go2.transform.position)));
    //}

    //[Server]
    //private bool CanCollectTarget(Transform transform)
    //{
    //    return (transform.position - this.transform.position).sqrMagnitude <=
    //        (3f) * (3f);
    //}

    //private void SetCollectingSpeed()
    //{
    //    var stats = GetComponent<LocalStats>().Stats;

    //    farmerSpeed = stats.GetAttributeAmount(AttributeType.Farmer);
    //    forageSpeed = stats.GetAttributeAmount(AttributeType.Forager);
    //    goldMinerSpeed = stats.GetAttributeAmount(AttributeType.GoldMiner);
    //    hunterSpeed = stats.GetAttributeAmount(AttributeType.Hunter);
    //    lumberjackSpeed = stats.GetAttributeAmount(AttributeType.Lumberjack);
    //    stoneMinerSpeed = stats.GetAttributeAmount(AttributeType.StoneMiner);
    //}

    //private void HandleAlteredStats(Stats stats)
    //{
    //    SetCollectingSpeed();
    //}

    //[ClientRpc]
    //private void ClientDebug(string message)
    //{
    //    Debug.Log(message);
    //}
}
