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

    [SerializeField]
    private List<Sprite> m_Sprites = new List<Sprite>();

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
                if (targeter.Attacker)
                {
                    selectedList.Add(unit.gameObject);
                }
            }
        }

        return selectedList.Count > 0;
    }

    public bool SelectedContainsBuilder()
    {
        var selectedList = new List<GameObject>();

        foreach (var unit in Selected)
        {
            if (unit.TryGetComponent(out Targeter targeter))
            {
                if (targeter.Builder)
                {
                    selectedList.Add(unit.gameObject);
                }
            }
        }

        return selectedList.Count > 0;
    }

    public bool SelectedContainsCollector()
    {
        var selectedList = new List<GameObject>();

        foreach (var unit in Selected)
        {
            if (unit.TryGetComponent(out Targeter targeter))
            {
                if (targeter.Collector)
                {
                    selectedList.Add(unit.gameObject);
                }
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

        foreach (var unit in player.DeployedUnits)
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

        foreach (var selectedUnit in Selected.Select(go => go.GetComponent<Unit>()))
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

            if (selected.TryGetComponent(out IBuilder builder))
            {
                CreateUnitBehaviours(builder.Buildings);
            }
            if (selected.TryGetComponent(out Spawner spawner))
            {
                CreateBuildingBehaviours(spawner.Units, spawner);
                CreateBuildingBehaviours(spawner.Upgrades, spawner);
            }
            return;
        }

        if (selectedList.Count > 0)
        {
            if (Utils.UnitsAreSameType(selectedList))
            {
                if (Selected[0].gameObject.TryGetComponent(out IBuilder builder))
                {
                    CreateUnitBehaviours(builder.Buildings);
                }
            }

            var unitList = new List<Unit>();

            foreach (var go in Selected)
            {
                if (go.TryGetComponent(out Unit unit))
                {
                    unitList.Add(unit);
                }
            }

            ArmyDisplay.Current.AddButtons(unitList.ToArray());
        }
    }

    private void AddBehaviours(ActionBehaviour[] behaviours)
    {
        if (behaviours.Length <= 0)
        {
            return;
        }

        var priorUpgrades = player.MyUpgrades;
        var buttonList = new List<ActionButton>();

        foreach (var behaviour in behaviours)
        {
            var showButton = true;

            var requiredUpgrades = behaviour.PriorUpgrades;

            foreach (var requiredUpgrade in requiredUpgrades)
            {
                if (!(priorUpgrades.Contains(requiredUpgrade)))
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
                buttonList.Add(new ActionButton(behaviour.Icon, behaviour.Description, behaviour.GetClickAction(), behaviour.Position));
            }
        }

        ActionsDisplay.Current.FillButtons(buttonList);
        ActionsDisplay.Current.ResetPanels();
    }

    private void CreateUnitBehaviours(CreateEntity[] entities)
    {
        var actionBehaviours = new List<ActionBehaviour>();

        foreach (var entity in entities)
        {
            if (entity.Object.TryGetComponent(out Building building))
            {
                actionBehaviours.Add(new CreateBuildingAction(player, building, entity.Position));
            }
        }

        if (actionBehaviours.Where(behaviour => behaviour.Position > 14).Any())
        {
            actionBehaviours.Add(new SwitchPanelsAction(m_Sprites[0], 14));
            actionBehaviours.Add(new SwitchPanelsAction(m_Sprites[0], 29));
        }

        AddBehaviours(actionBehaviours.ToArray());
    }

    private void CreateBuildingBehaviours(CreateEntity[] entities, Spawner spawner)
    {
        var actionBehaviours = new List<ActionBehaviour>();

        foreach (var entity in entities)
        {
            if (entity.Object.TryGetComponent(out Unit unit))
            {
                actionBehaviours.Add(new CreateUnitAction(player, unit, spawner, entity.Position));
            }
            if (entity.Object.TryGetComponent(out Upgrade upgrade))
            {
                actionBehaviours.Add(new CreateUpgradeAction(player, upgrade, spawner, entity.Position));
            }
        }

        if (actionBehaviours.Where(behaviour => behaviour.Position > 14).Any())
        {
            actionBehaviours.Add(new SwitchPanelsAction(m_Sprites[0], 15));
            actionBehaviours.Add(new SwitchPanelsAction(m_Sprites[0], 30));
        }

        AddBehaviours(actionBehaviours.ToArray());
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

            if (unit.TryGetComponent(out Builder builder))
            {

            }
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
