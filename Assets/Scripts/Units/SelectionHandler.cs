using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class SelectionHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask = new LayerMask();

    [SerializeField]
    private RectTransform selectionBox = null;

    [SerializeField]
    private List<GameObject> m_Selected = new List<GameObject>();

    private Vector2 startPosition;
    private RtsPlayer player;
    private Camera mainCamera;

    public List<GameObject> Selected
    {
        get { return m_Selected; }
        set { m_Selected = value; }
    }

    private void Start()
    {
        mainCamera = Camera.main;

        player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawn;
        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
    }

    private void OnDestroy()
    {
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawn;
        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && 
            !EventSystem.current.IsPointerOverGameObject())
        {
            StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            ClearSelectionArea();
            OrderList();
            AddNewUI();
        }
        else if (Mouse.current.leftButton.isPressed &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            UpdateSelectionArea();
        }
    }

    public void SelectSingleUnit(Unit unit)
    {
        ClearAndDeselect();
        SelectUnit(unit);
        AddNewUI();
    }

    public SelectedEntity CurrentSelectedEntity()
    {
        if (Selected.Count <= 0)
        {
            return SelectedEntity.None;
        }

        if (Selected[0].TryGetComponent(out Unit unit))
        {
            return SelectedEntity.Unit;
        }

        if (Selected[0].TryGetComponent(out Collectable collectable))
        {
            return SelectedEntity.Resource;
        }

        if (Selected[0].TryGetComponent(out Building building))
        {
            return SelectedEntity.Building;
        }

        return SelectedEntity.None;
    }

    public bool UnitsAreSameType()
    {
        var selectedList = Selected.Select(go => go.GetComponent<Unit>()).ToList();

        if (Utils.UnitsAreSameType(selectedList))
        {
            return true;
        }

        return false;
    }

    public EntityType SelectedEntityType()
    {
        var selectedList = Selected.Select(go => go.GetComponent<GameObjectIdentity>()).ToList();

        if (Utils.SelectedAreSameEntityType(selectedList))
        {
            return selectedList[0].EntityType;
        }

        return EntityType.Mixed;
    }

    public bool SelectedContainsUnit(EntityType type)
    {
        var selectedList = Selected.Select(go => go.GetComponent<Unit>().EntityType).ToList();

        return selectedList.Contains(type);
    }

    public bool SelectedContainsTargeter()
    {
        var selectedList = new List<GameObject>();

        foreach (var unit in Selected)
        {
            if (unit.TryGetComponent(out Targeter targeter))
            {
                selectedList.Add(targeter.gameObject);
            }
        }

        return selectedList.Count > 0;
    }

    public bool SelectedContainsBuilder()
    {
        var selectedList = new List<GameObject>();

        foreach (var unit in Selected)
        {
            if (unit.TryGetComponent(out Builder builder))
            {
                selectedList.Add(builder.gameObject);
            }
        }

        return selectedList.Count > 0;
    }

    public bool SelectedContainsCollector()
    {
        var selectedList = new List<GameObject>();

        foreach (var unit in Selected)
        {
            if (unit.TryGetComponent(out Collector collector))
            {
                selectedList.Add(collector.gameObject);
            }
        }

        return selectedList.Count > 0;
    }

    private void StartSelectionArea()
    {
        if (!Keyboard.current.leftShiftKey.isPressed)
        {
            ClearAndDeselect();
        }

        selectionBox.gameObject.SetActive(true);

        startPosition = Mouse.current.position.ReadValue();

        UpdateSelectionArea();
    }

    private void UpdateSelectionArea()
    {
        var mousePosition = Mouse.current.position.ReadValue();

        var areaWidth = mousePosition.x - startPosition.x;
        var areaHeight = mousePosition.y - startPosition.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));

        selectionBox.anchoredPosition = startPosition +
            new Vector2(areaWidth / 2, areaHeight / 2);
    }

    private void ClearSelectionArea()
    {
        selectionBox.gameObject.SetActive(false);

        if (selectionBox.sizeDelta.magnitude == 0)
        {
            var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                return;
            }

            if (hit.collider.TryGetComponent(out Building building))
            {
                SelectBuilding(building);
                return;
            }
            if (hit.collider.TryGetComponent(out Unit unit))
            {
                SelectUnit(unit);
                return;
            }
            if (hit.collider.TryGetComponent(out Collectable collectable))
            {
                SelectResource(collectable);
                return;
            }
        }

        var min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        var max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach (var unit in player.GetMyUnits())
        {
            if (Selected.Select(go => go.GetComponent<Unit>()).Contains(unit))
            {
                continue;
            }

            var screenPosition = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPosition.x > min.x &&
                screenPosition.x < max.x &&
                screenPosition.y > min.y &&
                screenPosition.y < max.y)
            {
                Selected.Add(unit.gameObject);
                unit.Select();
            }
        }
    }

    public void SelectUnit(Unit unit)
    {
        if (!unit.hasAuthority)
        {
            return;
        }

        Selected.Add(unit.gameObject);

        foreach (Unit selectedUnit in Selected.Select(go => go.GetComponent<Unit>()))
        {
            selectedUnit.Select();
        }

        return;
    }

    private void SelectBuilding(Building building)
    {
        if (!building.hasAuthority)
        {
            return;
        }

        Selected.Add(building.gameObject);
        building.Select();
        return;
    }

    private void SelectResource(Collectable collectable)
    {
        Selected.Add(collectable.gameObject);

        return;
    }

    private void OrderList()
    {
        if (CurrentSelectedEntity() == SelectedEntity.Unit)
        {
            Selected.OrderBy(o => o.GetComponent<GameObjectIdentity>().EntityType).ToList();
        }
    }

    private void AddNewUI()
    {
        ClearInfo();

        var selectedList = Selected.Select(go => go.GetComponent<Unit>()).ToList();

        if (selectedList.Count == 0)
        {
            return;
        }

        if (selectedList.Count == 1)
        {
            var selected = Selected[0];

            AddSingleEntityInformation(selected.gameObject);

            var entityType = SelectedEntityType();
            if (entityType == EntityType.House ||
                entityType == EntityType.Dropoff ||
                entityType == EntityType.Tower)
            {
                if (!selected.GetComponent<Building>().BuildingIsCompleted)
                {
                    return;
                }
            }

            var unitBehaviours = selected.GetComponents<ActionBehaviour>();
            
            AddBehaviours(unitBehaviours);

            return;
        }

        if (selectedList.Count > 0)
        {
            if (Utils.UnitsAreSameType(selectedList))
            {
                var unitBehaviours = Selected[0].gameObject.GetComponents<ActionBehaviour>();
                AddBehaviours(unitBehaviours);
            }

            foreach (var unit in Selected)
            {
                AddArmyInformation(unit.gameObject.GetComponent<Unit>());
            }
            return;
        }


        var behaviours = Selected.Select(go => go.GetComponent<Unit>()).FirstOrDefault().gameObject.GetComponents<ActionBehaviour>();

        AddBehaviours(behaviours);
    }

    private void AddBehaviours(ActionBehaviour[] behaviours)
    {
        var priorUpgrades = player.GetMyUpgrades();
        foreach (var behaviour in behaviours)
        {
            var showButton = true;

            var neededUpgrades = behaviour.PriorUpgrades;

            foreach (var neededUpgrade in neededUpgrades)
            {
                if (!(priorUpgrades.Contains(neededUpgrade)))
                {
                    showButton = false;
                }
            }

            if (priorUpgrades.Contains(behaviour.Id))
            {
                showButton = false;
            }

            if (showButton)
            {
                ActionsDisplay.Current.AddButton(
                behaviour.Icon,
                behaviour.GetClickAction(),
                behaviour.Position);
            }
        }
    }

    private void AddArmyInformation(Unit unit)
    {
        ArmyDisplay.Current.AddButton(unit);
    }

    private void AddSingleEntityInformation(GameObject go)
    {
        SingleDisplay.Current.AddEntity(go);
    }

    private void ClearAndDeselect()
    {
        foreach (var unit in Selected)
        {
            unit.GetComponent<Interactable>().Deselect();
        }

        Selected.Clear();
    }

    private void ClearInfo()
    {
        ActionsDisplay.Current.ClearButtons();
        ArmyDisplay.Current.ClearButtons();
        SingleDisplay.Current.ClearInfo();
    }

    private void AuthorityHandleUnitDespawn(Unit unit)
    {
        Selected.Remove(unit.gameObject);
    }

    private void ClientHandleGameOver(string winner)
    {
        enabled = false;
    }
}
