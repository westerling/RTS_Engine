using System.Collections.Generic;
using UnityEngine;

public static class TargetFinder
{
    public static Targetable FindNewBuilding(RtsPlayer player, Transform transform)
    {
        var constructions = player.Constructions;

        if (constructions.Count == 0)
        {
            return null;
        }

        var closestConstruction = constructions[0];
        var distance = Vector3.Distance(closestConstruction.gameObject.transform.position, transform.position);

        foreach (var construction in constructions)
        {
            var newDistance = Vector3.Distance(construction.gameObject.transform.position, transform.position);

            if (newDistance < distance)
            {
                closestConstruction = construction;
                distance = newDistance;
            }
        }

        return closestConstruction;
    }

    public static Targetable FindNewEnemyUnit(Transform transform, float range)
    {
        var unitArray = GameObject.FindGameObjectsWithTag("Unit");
        var unitList = new List<Targetable>();

        foreach (var unit in unitArray)
        {
            if (unit.TryGetComponent(out Targetable targetable))
            {
                if (!targetable.hasAuthority)
                {
                    if (Utils.IsCloseEnough(unit.GetComponent<Interactable>(), transform.position, range))
                    {
                        unitList.Add(targetable);
                    }
                }
            }
        }

        if (unitList.Count == 0)
        {
            return null;
        }

        unitList.Sort((go1, go2) => Vector3.Distance(transform.position, go1.transform.position).CompareTo(Vector3.Distance(transform.position, go2.transform.position)));

        return unitList[0];
    }

    public static Collectable FindNewResource(Transform transform, Resource currentResource)
    {
        var resourceArray = GameObject.FindGameObjectsWithTag("Resource");
        var resourceList = new List<Collectable>();

        foreach (var go in resourceArray)
        {
            if (go.TryGetComponent(out Collectable collectable))
            {
                if (collectable.Resource == currentResource)
                {
                    resourceList.Add(collectable);
                }
            }
        }

        if (resourceList.Count == 0)
        {
            return null;
        }

        resourceList.Sort((go1, go2) => Vector3.Distance(transform.position, go1.transform.position).CompareTo(Vector3.Distance(transform.position, go2.transform.position)));

        return resourceList[0];
    }

    public static 
}
