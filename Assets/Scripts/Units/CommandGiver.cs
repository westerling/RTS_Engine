using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using System.Collections.Generic;
using System.Linq;

public class CommandGiver : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_LayerMask = new LayerMask();

    [SerializeField]
    private LayerMask m_CursorLayerMask = new LayerMask();

    [SerializeField]
    private SelectionHandler m_SelectionHandler = null;

    [SerializeField]
    private GameObject m_Formation = null;

    private Camera mainCamera;
    private CursorManager m_CursorManager = null;

    private void Start()
    {
        mainCamera = Camera.main;
        m_CursorManager = NetworkClient.connection.identity.GetComponent<CursorManager>();
        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
    }

    private void OnDestroy()
    {
        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;
    }

    private void Update()
    {
        UpdateCursor();

        if (!Mouse.current.rightButton.wasPressedThisFrame)
        {
            return;
        }

        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_LayerMask))
        {
            return;
        }

        ResetTargets();

        switch (m_SelectionHandler.CurrentSelectedEntity())
        {
            case SelectedEntity.Unit:
                UnitSelected(hit);
                break;
            case SelectedEntity.Building:
                BuildingSelected(hit);
                break;
            default:
                break;
        }
    }

    private void UnitSelected(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out Targetable target))
        {
            if (!target.hasAuthority)
            {
                TryTarget(target);
                return;
            }
        }

        if (hit.collider.TryGetComponent(out Building building))
        {
            if (building.hasAuthority)
            {
                if (building.TryGetComponent(out Health health))
                {
                    if (!health.HasFullHealth())
                    {
                        TryBuild(building);
                        return;
                    }
                }

                if (building.TryGetComponent(out DropOff dropOff))
                {
                    if (dropOff.GetComponent<Building>().BuildingIsCompleted)
                    {
                        TryDeliver(dropOff);
                        return;
                    }

                }
                else if (building.TryGetComponent(out TownCenter townCenter))
                {
                    if (townCenter.GetComponent<Building>().BuildingIsCompleted)
                    {
                        TryDeliver(townCenter);
                        return;
                    }
                }
            }
        }

        if (hit.collider.TryGetComponent(out Collectable resource))
        {
            TryCollect(resource);
            return;
        }

        TryMove(hit.point);
    }
    
    private void BuildingSelected(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out Targetable target))
        {
            if (!target.hasAuthority)
            {
                TryTarget(target);
                return;
            }
        }

        TrySetRallyPoint(hit.point);
    }

    private void ResetTargets()
    {
        var unitList = m_SelectionHandler.Selected.Select(go => go.GetComponent<Targetable>()).ToList();

        foreach (Targetable go in unitList)
        {
            go.Targeter?.CmdClearTarget();
            if (go.TryGetComponent(out Unit unit))
            {
                unit.Builder?.CmdClearTarget();
                unit.Collector?.CmdClearTarget();
            }
        }
    }

    private void TrySetRallyPoint(Vector3 point)
    {
        var buildingList = m_SelectionHandler.Selected.Select(go => go.GetComponent<Building>()).ToList();

        foreach (Building building in buildingList)
        {
            if (building.TryGetComponent(out Spawner spawner))
            {
                spawner.RallyPoint.transform.position = point;
            }
        }
    }

    private void TryMove(Vector3 point)
    {
        var unitList = m_SelectionHandler.Selected.Select(go => go.GetComponent<Unit>()).ToList();

        if (unitList.Count > 1)
        {
            MoveArmy(unitList, point);
        }
        else
        {
            MoveSingleUnit(unitList[0], point);
        }
    }

    private void MoveArmy(List<Unit> unitList, Vector3 point)
    {
        var formation = Instantiate(m_Formation, point, Quaternion.identity);
        formation.transform.LookAt(unitList[0].transform.position);
        

        for (var i = 0; i < unitList.Count; i++)
        {
            unitList[i].UnitMovement.CmdSetTask((int)Task.Move);
            if (Keyboard.current[Key.LeftShift].isPressed)
            {
                unitList[i].UnitMovement.CmdAddPoint(formation.GetComponent<Formation>().Transforms[i].position);
            }
            else
            {
                unitList[i].UnitMovement.CmdMove(formation.GetComponent<Formation>().Transforms[i].position);
            }
            
        }
        Destroy(formation);
    }

    private void MoveSingleUnit(Unit unit, Vector3 point)
    {
        unit.UnitMovement.CmdSetTask((int)Task.Move);
        if (Keyboard.current[Key.LeftShift].isPressed)
        {
            unit.UnitMovement.CmdAddPoint(point);
        }
        else
        {
            unit.UnitMovement.CmdMove(point);
        }
    }

    private void TryTarget(Targetable target)
    {
        var selectedList = m_SelectionHandler.Selected.Select(go => go.GetComponent<Targetable>()).ToList();

        foreach (Targetable go in selectedList)
        {
            var targeter = go.Targeter;

            if (targeter == null)
            {
                return;
            }

            targeter.CmdSetTarget(target.gameObject);

            if (go.TryGetComponent(out Unit unit))
            {
                unit.UnitMovement.CmdAttack();
            }
        }
    }

    private void TryCollect(Collectable resource)
    {
        var unitList = m_SelectionHandler.Selected.Select(go => go.GetComponent<Unit>()).ToList();

        foreach (Unit unit in unitList)
        {
            var collector = unit.Collector;

            if (collector == null)
            {
                return;
            }
            var closestDropoff = FindClosestDropoff(resource.Resource);

            collector.CmdSetTarget(resource.gameObject);
            collector.CmdSetDropOff(closestDropoff);

            unit.UnitMovement.CmdCollect();
        }
    }

    private void TryDeliver(DropOff dropOff)
    {
        var unitList = m_SelectionHandler.Selected.Select(go => go.GetComponent<Unit>()).ToList();

        foreach (Unit unit in unitList)
        {
            var collector = unit.Collector;

            if (collector == null)
            {
                return;
            }

            collector.CmdSetDropOff(dropOff.gameObject);

            unit.UnitMovement.CmdDeliver();
        }
    }

    private void TryDeliver(TownCenter townCeter)
    {
        var unitList = m_SelectionHandler.Selected.Select(go => go.GetComponent<Unit>()).ToList();

        foreach (Unit unit in unitList)
        {
            var collector = unit.Collector;

            if (collector == null)
            {
                return;
            }

            unit.UnitMovement.CmdSetTask((int)Task.Deliver);
            unit.UnitMovement.CmdMove(townCeter.gameObject.transform.position);
            collector.CmdSetDropOff(townCeter.gameObject);
        }
    }

    private void TryBuild(Building building)
    {
        var unitList = m_SelectionHandler.Selected.Select(go => go.GetComponent<Unit>()).ToList();

        foreach (Unit unit in unitList)
        {
            var builder = unit.Builder;

            if (builder == null)
            {
                return;
            }

            builder.CmdSetTarget(building.gameObject);
            unit.UnitMovement.CmdSetTask((int)Task.Build);
            unit.UnitMovement.CmdBuild();
            
        }
    }

    private void ClientHandleGameOver(string winner)
    {
        enabled = false;
    }

    private GameObject FindClosestDropoff(Resource resource)
    {
        var player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        var dropOffList = new List<GameObject>();

        foreach (var building in player.GetMyBuildings())
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
                    dropOffList.Add(dropOff.gameObject);
                    continue;
                }
            }
            if (building.TryGetComponent(out TownCenter townCenter))
            {
                dropOffList.Add(townCenter.gameObject);
            }
        }

        if (dropOffList.Count == 0)
        {
            return null;
        }

        return dropOffList.OrderBy(o => (o.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
    }

    private void UpdateCursor()
    {
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_CursorLayerMask))
        {
            m_CursorManager.SetCursorStyle(CursorType.Normal);
            return;
        }

        if (m_SelectionHandler.SelectedContainsTargeter())
        {
            if (hit.collider.TryGetComponent(out Targetable target))
            {
                if (!target.hasAuthority)
                {
                    m_CursorManager.SetCursorStyle(CursorType.Attack);
                    return;
                }
            }
        }

        if (m_SelectionHandler.SelectedContainsBuilder())
        {
            if (hit.collider.TryGetComponent(out Building building))
            {
                if (building.hasAuthority)
                {
                    if (building.TryGetComponent(out Health health))
                    {
                        if (!health.HasFullHealth())
                        {
                            m_CursorManager.SetCursorStyle(CursorType.Build);
                            return;
                        }
                    }
                }
            }
        }

        if (m_SelectionHandler.SelectedContainsCollector())
        {
            if (hit.collider.TryGetComponent(out Collectable resource))
            {
                switch (resource.Resource)
                {
                    case Resource.Food:
                        m_CursorManager.SetCursorStyle(CursorType.CollectFood);
                        break;
                    case Resource.Gold:
                    case Resource.Stone:
                        m_CursorManager.SetCursorStyle(CursorType.Mine);
                        break;
                    case Resource.Wood:
                        m_CursorManager.SetCursorStyle(CursorType.Axe);
                        break;
                    default:
                        break;
                }
                return;
            }
        }   
    }
}
