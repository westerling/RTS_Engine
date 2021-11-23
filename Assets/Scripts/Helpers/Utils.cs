using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Utils : NetworkBehaviour
{
    public static bool CanAfford(IDictionary<Resource, int> availableResources, IDictionary<Resource, int> cost)
    {
        foreach (var resourceCostItem in cost)
        {
            if (availableResources[resourceCostItem.Key] < resourceCostItem.Value)
            {
                return false;
            }
        }
        return true;
    }

    public static bool SelectedAreSameType(List<GameObjectIdentity> selectedEntities)
    {

        if (selectedEntities.Count < 2)
        {
            return true;
        }

        var comparisonUnit = selectedEntities[0];

        foreach (var unit in selectedEntities)
        {
            if (unit.Id != comparisonUnit.Id)
            {
                return false;
            }
        }

        return true;
    }

    public static bool SelectedAreSameEntityType(List<GameObjectIdentity> selectedEntities)
    {

        if (selectedEntities.Count < 2)
        {
            return true;
        }

        var comparisonUnit = selectedEntities[0];

        foreach (var unit in selectedEntities)
        {
            if (unit.EntityType != comparisonUnit.EntityType)
            {
                return false;
            }
        }

        return true;
    }


    public static bool UnitsAreSameType(List<Unit> unitList)
    {
    
        if (unitList.Count < 2)
        {
            return true;
        }

        var comparisonUnit = unitList[0];

        foreach (var unit in unitList)
        {
            if (unit.Id != comparisonUnit.Id)
            {
                return false;
            }
        }

        return true;
    }

    public static List<GameObject> GameObjectToList(GameObject gameObject)
    {
        var list = new List<GameObject>();

        list.Add(gameObject);

        return list;
    }

    public static List<GameObject> BuildingToGameObjectList(Building building)
    {
        var list = new List<GameObject>();

        list.Add(building.gameObject);

        return list;
    }

    public static List<GameObject> UnitToGameObjectList(Unit unit)
    {
        var list = new List<GameObject>();

        list.Add(unit.gameObject);

        return list;
    }

    public static List<GameObject> UnitListToGameObjectList(List<Unit> unitList)
    {
        var list = new List<GameObject>();


        foreach(var unit in unitList)
        {
            list.Add(unit.gameObject);
        }

        return list;
    }

    public static float Round(float value, int digits)
    {
        var mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    public static T Deserialize<T>(Stream stream)
    where T : class
    {
        try
        {
            var xmlSettings = new XmlReaderSettings
            {
                IgnoreWhitespace = true
            };

            var xmlReader = XmlReader.Create(stream, xmlSettings);
            var ser = new XmlSerializer(typeof(T));

            if (ser.CanDeserialize(xmlReader))
            {
                var result = ser.Deserialize(xmlReader) as T;
                return result;
            }
        }
        catch (Exception)
        {
            Debug.LogError("Cannot Deserialize");
        }

        return default(T);
    }

    public static Vector3 MiddleOfScreenPointToWorld()
    {
        return MiddleOfScreenPointToWorld(Camera.main, Globals.FlatTerrainLayerMask);
    }

    public static Vector3 MiddleOfScreenPointToWorld(LayerMask layerMask)
    { 
        return MiddleOfScreenPointToWorld(Camera.main, layerMask);
    }

    public static Vector3 MiddleOfScreenPointToWorld(Camera cam, LayerMask layerMask)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(0.5f * new Vector2(Screen.width, Screen.height));
        if (Physics.Raycast(
                ray,
                out hit,
                1000f,
                layerMask
            ))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public static Vector3[] ScreenCornersToWorldPoints()
    { 
        return ScreenCornersToWorld(Camera.main);
    }

    public static Vector3[] ScreenCornersToWorld(Camera cam)
    {
        Vector3[] corners = new Vector3[4];
        RaycastHit hit;
        for (int i = 0; i < 4; i++)
        {
            Ray ray = cam.ScreenPointToRay(new Vector2((i % 2) * Screen.width, (int)(i / 2) * Screen.height));
            if (Physics.Raycast(
                    ray,
                    out hit,
                    1000f,
                    Globals.FlatTerrainLayerMask
                )) corners[i] = hit.point;
        }
        return corners;
    }

    public static IEnumerator BlinkGameObject(GameObject gameObject, int numBlinks, float seconds)
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        
        for (int i = 0; i < numBlinks * 2; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(seconds);
        }

        renderer.enabled = true;
    }

    public static Vector3 OffsetArmy(Vector3 startPos, Vector3 endPos, int offset)
    {
        var distanceVector = startPos - endPos;
        var distanceVectorNormalized = distanceVector.normalized;
        var targetPosition = (distanceVectorNormalized * 5f);
        return targetPosition;
    }

    public static Vector3 OffsetPoint(Vector3 startPoint, int offset)
    {
        startPoint.x += UnityEngine.Random.Range(-offset, offset);
        startPoint.z += UnityEngine.Random.Range(-offset, offset);

        return startPoint;
    }

    public static float AtBuildingEdge(Building building)
    {
        var size = building.Size;

        return building.gameObject.GetComponent<Collider>().bounds.max.x / 2;
    }

    public static float DistanceToBuilding(EntitySize size)
    {
        switch (size)
        {
            case EntitySize.Tiny:
                return 1f;
            case EntitySize.Small:
                return 2.5f;
            case EntitySize.Normal:
                return 3f;
            case EntitySize.Large:
                return 4f;
            case EntitySize.Huge:
                return 5f;
            default:
                return 2f;
        }
    }

    public bool IsCloseEnough(Vector3 targetPosition, float size)
    {
        return (targetPosition - transform.position).sqrMagnitude <=
            (size) * (size);
    }

    public static int SortByDistance(Vector3 pos, GameObject go1, GameObject go2)
    {
        return Vector3.Distance(pos, go1.transform.position).CompareTo(Vector3.Distance(pos, go2.transform.position));
    }
}
