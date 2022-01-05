using System.Collections.Generic;
using UnityEngine;

public static class TargetFinder
{
    public static InteractableGameEntity FindNewBuilding(RtsPlayer player, Transform transform)
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

    public static InteractableGameEntity FindNewEnemyUnit(Transform transform, float range)
    {
        var unitArray = GameObjectArray("Unit");

        if (unitArray.Length > 0)
        {
            var unitList = GetClosestEntity(transform, range, unitArray);

            unitList.Sort((go1, go2) => Vector3.Distance(transform.position, go1.transform.position).CompareTo(Vector3.Distance(transform.position, go2.transform.position)));

            return unitList[0];
        }
        else
        {
            var buildingArray = GameObjectArray("Building");

            if (buildingArray.Length > 0)
            {
                var buildingList = GetClosestEntity(transform, range, buildingArray);

                buildingList.Sort((go1, go2) => Vector3.Distance(transform.position, go1.transform.position).CompareTo(Vector3.Distance(transform.position, go2.transform.position)));

                return buildingList[0];
            }
        }

        return null;
    }

    private static List<InteractableGameEntity> GetClosestEntity(Transform transform, float range, GameObject[] gameObjectArray)
    {
        var entityList = new List<InteractableGameEntity>();

        foreach (var go in gameObjectArray)
        {
            if (go.TryGetComponent(out InteractableGameEntity targetable))
            {
                if (!targetable.hasAuthority)
                {
                    if (Utils.IsCloseEnough(go.GetComponent<Interactable>(), transform.position, range))
                    {
                        entityList.Add(targetable);
                    }
                }
            }
        }

        return entityList;
    }

    public static Collectable FindNewResource(Transform transform, Resource currentResource)
    {
        var resourceArray = GameObjectArray("Resource");
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

    public static InteractableGameEntity FindClosestDropoff(RtsPlayer player, Transform transform, Resource resource)
    {
        var dropOffList = new List<InteractableGameEntity>();

        foreach (var building in player.DeployedBuildings)
        {
            if (!building.hasAuthority)
            {
                continue;
            }

            if (!building.BuildingIsCompleted)
            {
                continue;
            }

            if (building.TryGetComponent(out DropOff dropOff))
            {
                if (dropOff.Resource == resource)
                {
                    dropOffList.Add(dropOff);
                    continue;
                }
            }
            if (building.TryGetComponent(out TownCenter townCenter))
            {
                dropOffList.Add(townCenter);
            }
        }

        if (dropOffList.Count == 0)
        {
            return null;
        }

        return GetClosestFromList(transform, dropOffList);
    }

    private static InteractableGameEntity GetClosestFromList(Transform transform, List<InteractableGameEntity> interactableList)
    {
        InteractableGameEntity closest = null;
        var minDist = Mathf.Infinity;

        foreach (var interactable in interactableList)
        {
            var dist = Vector3.Distance(interactable.transform.position, transform.position);
            if (dist < minDist)
            {
                closest = interactable;
                minDist = dist;
            }
        }
        return closest;
    }

    private static GameObject[] GameObjectArray(string tag)
    {
        return GameObject.FindGameObjectsWithTag(tag);
    }
}
