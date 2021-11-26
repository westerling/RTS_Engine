using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RtsPlayer : NetworkBehaviour
{
    [SerializeField]
    private LayerMask m_BuildingBlockLayer = new LayerMask();

    [SerializeField]
    private Transform m_MainCameraTransform = null;

    [SerializeField]
    private Building[] m_Buildings = new Building[0];

    [SerializeField]
    private Unit[] m_Units = new Unit[0];

    [SerializeField]
    private Upgrade[] m_Upgrades = new Upgrade[0];

    [SyncVar(hook = nameof(ClientHandleFoodUpdated))]
    private int m_Food;

    [SyncVar(hook = nameof(ClientHandleGoldUpdated))]
    private int m_Gold;

    [SyncVar(hook = nameof(ClientHandleStoneUpdated))]
    private int m_Stone;

    [SyncVar(hook = nameof(ClientHandleWoodUpdated))]
    private int m_Wood;

    [SyncVar(hook = nameof(ClientHandleCurrentPopulationUpdated))]
    private int m_CurrentPopulation;

    [SyncVar(hook = nameof(ClientHandleMaximumPopulationUpdated))]
    private int m_MaxPopulation;

    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    private bool m_IsPartyOwner = false;

    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    private string m_DisplayName;

    [SerializeField]
    private List<int> myUpgrades = new List<int> {0};

    
    public event Action<int, Resource> ClientOnResourcesUpdated;
    public event Action<int> ClientOnCurrentPopulationUpdated;
    public event Action<int> ClientOnMaximumPopulationUpdated;
    
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;
    public static event Action ClientOnInfoUpdated;
    public static GameContext GameState = GameContext.Camera;
    
    private Faction m_Faction;
    private List<GameObjectIdentity> m_Researchables = new List<GameObjectIdentity>();
    private List<Unit> m_DeployedUnits = new List<Unit>();
    private List<Building> m_DeployedBuildings = new List<Building>();
    private List<Building> m_Constructions = new List<Building>();
    private Color m_TeamColor = new Color();

    public int CurrentPopulation
    {
        get => m_CurrentPopulation;
        set => m_CurrentPopulation = value;
    }

    public int MaxPopulation
    {
        get => m_MaxPopulation;
        set => m_MaxPopulation = value;
    }

    public string DisplayName
    {
        get => m_DisplayName;
        set => m_DisplayName = value;
    }

    public Transform MainCameraTransform
    {
        get => m_MainCameraTransform;
        set => m_MainCameraTransform = value;
    }

    public Color TeamColor
    {
        get => m_TeamColor;
        set => m_TeamColor = value;
    }

    public Faction Faction
    {
        get => m_Faction;
        set => m_Faction = value;
    }

    public List<Building> DeployedBuildings
    {
        get => m_DeployedBuildings;
    }

    public List<Building> Constructions
    {
        get => m_Constructions;
    }


    public List<Unit> DeployedUnits
    {
        get => m_DeployedUnits;
    }

    public List<int> MyUpgrades
    {
        get => myUpgrades;
    }

    public bool IsPartyOwner
    {
        get => m_IsPartyOwner;
        set => m_IsPartyOwner = value;
    }

    public IDictionary<Resource, int> GetResources()
    {
        IDictionary<Resource, int> d = new Dictionary<Resource, int>();
        d.Add(new KeyValuePair<Resource, int>(Resource.Food, m_Food));
        d.Add(new KeyValuePair<Resource, int>(Resource.Gold, m_Gold));
        d.Add(new KeyValuePair<Resource, int>(Resource.Stone, m_Stone));
        d.Add(new KeyValuePair<Resource, int>(Resource.Wood, m_Wood));

        return d;
    }

    [Server]
    public void AddUpgrade(int upgradeId)
    {
        myUpgrades.Add(upgradeId);
    }

    public GameObject GetGameobjectFromId(int id)
    {
        foreach (var researchable in m_Researchables)
        {
            if (researchable.Id == id)
            {
                return researchable.gameObject;
            }
        }

        foreach (var building in m_Buildings)
        {
            if (building.Id == id)
            {
                return building.gameObject;
            }
        }

        foreach (var unit in m_Units)
        {
            if (unit.Id == id)
            {
                return unit.gameObject;
            }
        }

        foreach (var upgrade in m_Upgrades)
        {
            if (upgrade.Id == id)
            {
                return upgrade.gameObject;
            }
        }

        return null;
    }

    public Unit GetUnitFromId(int id)
    {
        foreach (var unit in m_Units)
        {
            if (unit.Id == id)
            {
                return unit;
            }
        }
        return null;
    }

    public Building GetBuildingFromId(int id)
    {
        foreach (var building in m_Buildings)
        {
            if (building.Id == id)
            {
                return building;
            }
        }
        return null;
    }

    public Upgrade GetUpgradeFromId(int id)
    {
        foreach (var upgrade in m_Upgrades)
        {
            if (upgrade.Id == id)
            {
                return upgrade;
            }
        }
        return null;
    }

    public bool CanAffordBuilding(Building building)
    {
        var cost = building.LocalStats.Stats.GetCost();
        var resources = GetResources();

        if (!Utils.CanAfford(resources, cost))
        {
            return false;
        }

        return true;
    }

    public bool CanPlaceBuilding(BoxCollider collider, Vector3 point)
    {
        if (Physics.CheckBox(
            point + collider.center,
            collider.size / 2,
            Quaternion.identity,
            m_BuildingBlockLayer))
        {
            return false;
        }
        
        return true;
    }

    public bool CanPlaceTower(BoxCollider collider, Vector3 point)
    {
        if (Physics.CheckBox(
            point + collider.center,
            collider.size / 2,
            Quaternion.identity,
            m_BuildingBlockLayer))
        {
            
            return false;
        }

        return true;
    }

    #region server

    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandleUnitsSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleUnitsDespawned;

        Building.ServerOnConstructionStarted += ServerHandleConstructionStarted;
        Building.ServerOnBuildingCompleted += ServerHandleBuildingCompleted;
        Building.ServerOnBuildingDespawned += ServerHandleBuildingsDespawned;

        Upgrade.ServerOnUpgradeAdded += ServerHandleUpgradeAdded;

        SetStartResources();

        SetupResearchables();

        DontDestroyOnLoad(gameObject);
    }

    private void SetStartResources()
    {
        m_Food = 5000;
        m_Gold = 5000;
        m_Stone = 5000;
        m_Wood = 5000;
    }

    private void SetupResearchables()
    {
        foreach (var unit in m_Units)
        {
            if (unit.TryGetComponent(out GameObjectIdentity researchable))
            {
                m_Researchables.Add(researchable);
            }
        }

        foreach (var building in m_Buildings)
        {
            if (building.TryGetComponent(out GameObjectIdentity researchable))
            {
                m_Researchables.Add(researchable);
            }
        }

        foreach (var upgrade in m_Upgrades)
        {
            if (upgrade.TryGetComponent(out GameObjectIdentity researchable))
            {
                m_Researchables.Add(researchable);
            }
        } 
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitsSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitsDespawned;

        Building.ServerOnConstructionStarted -= ServerHandleConstructionStarted;
        Building.ServerOnBuildingDespawned -= ServerHandleBuildingsDespawned;
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.m_DisplayName = displayName;
    }

    [Server]
    public void SetPartyOwner(bool state)
    {
        IsPartyOwner = state;
    }

    [Server]
    public void SetTeamColor(Color newTeamColor)
    {
        TeamColor = newTeamColor;
    }

    [Server]
    public void SetFaction(Faction newFaction)
    {
        Faction = newFaction;
    }

    [Command]
    public void CmdSetMaximumPopulation(int population)
    {
        SetMaximumPopulation(population);
    }

    [Server]
    public void SetMaximumPopulation(int population)
    {
        MaxPopulation += population;
    }

    [Command]
    public void CmdSetCurrentPopulation(int population)
    {
        SetCurrentPopulation(population);
    }

    [Server]
    public void SetCurrentPopulation(int population)
    {
        CurrentPopulation += population;
    }

    [Command]
    public void CmdSetResources(int resourceType, int amount)
    {
        SetResources(resourceType, amount);
    }

    [Server]
    public void SetResources(int resourceType, int amount)
    {
        switch ((Resource)resourceType)
        {
            case Resource.Food:
                m_Food += amount;
                break;
            case Resource.Gold:
                m_Gold += amount;
                break;
            case Resource.Stone:
                m_Stone += amount;
                break;
            case Resource.Wood:
                m_Wood += amount;
                break;
        }
    }

    [Command]
    public void CmdStartGame(int mapId)
    {
        if (!IsPartyOwner)
        {
            return;
        }

        ((RtsNetworkManager)NetworkManager.singleton).StartGame(mapId);
    }

    [Command]
    public void CmdTryPlaceBuilding(int buildingId, Vector3 point, Quaternion rotation)
    {
        Building buildingToPlace = null;

        foreach (var building in m_Buildings)
        {
            if (building.Id == buildingId)
            {
                buildingToPlace = building;
                break;
            }
        }

        if (buildingToPlace == null)
        {
            return;
        }
        
        if (!CanAffordBuilding(buildingToPlace))
        {
            return;
        }

        var collider = buildingToPlace.GetComponent<BoxCollider>();

        if (!CanPlaceBuilding(collider, point))
        {
            return;
        }

        var buildingInstance = Instantiate(
            buildingToPlace.gameObject, 
            point,
            rotation);

        NetworkServer.Spawn(buildingInstance, connectionToClient);

        var cost = buildingToPlace.LocalStats.Stats.GetCost();

        foreach (var resourceCostItem in cost)
        {
            SetResources((int)resourceCostItem.Key, -cost[resourceCostItem.Key]);
        }

        buildingInstance.GetComponent<Building>().InitializeConstruction();
    }

    private void ServerHandleUpgradeAdded(Upgrade upgrade)
    {
        if (upgrade.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        myUpgrades.Add(upgrade.Id);

        UpgradeCheck(upgrade);
    }

    private void UpgradeCheck(Upgrade newUpgrade)
    {
        foreach (var affectedEntity in newUpgrade.AffectedEntities)
        {
            if (affectedEntity.AffectedGameObject.TryGetComponent(out Building building))
            {
                m_Buildings.First(x => x == building).GetComponent<LocalStats>().AlterStats(affectedEntity.Stats);
            }
            if (affectedEntity.AffectedGameObject.TryGetComponent(out Unit unit))
            {
                m_Units.First(x => x == unit).GetComponent<LocalStats>().AlterStats(affectedEntity.Stats);
            }
            if (affectedEntity.AffectedGameObject.TryGetComponent(out Upgrade upgrade))
            {
                m_Upgrades.First(x => x == upgrade).GetComponent<LocalStats>().AlterStats(affectedEntity.Stats);
            }
        }
    }

    private void ServerHandleUnitsSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }
    
        if (!hasAuthority)
        {
            return;
        }

        DeployedUnits.Add(unit);
        SetCurrentPopulation(1);
    }    
    
    private void ServerHandleUnitsDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        DeployedUnits.Remove(unit);
        SetCurrentPopulation(-1);
    }

    private void ServerHandleBuildingCompleted(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        Constructions.Remove(building);
        DeployedBuildings.Add(building);

        if (!hasAuthority)
        {
            return;
        }

        SetMaximumPopulation((int)building.LocalStats.Stats.GetAttributeAmount(AttributeType.Population));
    }

    private void ServerHandleConstructionStarted(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        Constructions.Add(building);
    }    
    
    private void ServerHandleBuildingsDespawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        DeployedBuildings.Remove(building);

        SetMaximumPopulation(-(int)building.LocalStats.Stats.GetAttributeAmount(AttributeType.Population));
    }

    #endregion

    #region client

    private void ClientHandleFoodUpdated(int oldResources, int newResources)
    {
        ClientOnResourcesUpdated?.Invoke(newResources, Resource.Food);
    }

    private void ClientHandleGoldUpdated(int oldResources, int newResources)
    {
        ClientOnResourcesUpdated?.Invoke(newResources, Resource.Gold);
    }

    private void ClientHandleStoneUpdated(int oldResources, int newResources)
    {
        ClientOnResourcesUpdated?.Invoke(newResources, Resource.Stone);
    }

    private void ClientHandleWoodUpdated(int oldResources, int newResources)
    {
        ClientOnResourcesUpdated?.Invoke(newResources, Resource.Wood);
    }

    private void ClientHandleCurrentPopulationUpdated(int oldPopulation, int newPopulation)
    {
        ClientOnCurrentPopulationUpdated?.Invoke(newPopulation);
    }

    private void ClientHandleMaximumPopulationUpdated(int oldPopulation, int newPopulation)
    {
        ClientOnMaximumPopulationUpdated?.Invoke(newPopulation);
    }

    public override void OnStartAuthority()
    {
        if(NetworkServer.active)
        {
            return;
        }

        Upgrade.AuthorityOnUpgradeAdded += AuthorityHandleUpgradeAdded;

        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;

        Building.AuthorityOnConstructionStarted += AuthorityHandleConstructionStarted;
        Building.AuthorityOnBuildingCompleted += AuthorityHandleBuildingCompleted;
        Building.AuthorityOnBuildingDespawned += AuthorityHandleBuildingsDespawned;
    }

    public override void OnStartClient()
    {
        if (NetworkServer.active)
        {
            return;
        }

        DontDestroyOnLoad(gameObject);

        ((RtsNetworkManager)NetworkManager.singleton).Players.Add(this);
    }

    public override void OnStopClient()
    {
        
        if (isClientOnly)
        {
            ((RtsNetworkManager)NetworkManager.singleton).Players.Remove(this);
            ClientOnInfoUpdated?.Invoke();
        }
        else
        {
            ClientOnInfoUpdated?.Invoke();
            return;
        }

        if (!hasAuthority)
        {
            return;
        }

        Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;

        Building.AuthorityOnConstructionStarted -= AuthorityHandleConstructionStarted;
        Building.AuthorityOnBuildingDespawned -= AuthorityHandleBuildingsDespawned;
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!hasAuthority)
        {
            return;
        }

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }

    private void AuthorityHandleUnitSpawned(Unit unit)
    {
        DeployedUnits.Add(unit);
        CmdSetCurrentPopulation(1);
    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        DeployedUnits.Remove(unit);
        CmdSetCurrentPopulation(-1);
    }

    private void AuthorityHandleConstructionStarted(Building building)
    {
        Constructions.Add(building);
    }

    private void AuthorityHandleBuildingCompleted(Building building)
    {
        Constructions.Remove(building);
        DeployedBuildings.Add(building);
        CmdSetMaximumPopulation((int)building.LocalStats.Stats.GetAttributeAmount(AttributeType.Population));
    }

    private void AuthorityHandleBuildingsDespawned(Building building)
    {
        DeployedBuildings.Remove(building);

        CmdSetMaximumPopulation(-(int)building.LocalStats.Stats.GetAttributeAmount(AttributeType.Population));
    }

    private void AuthorityHandleUpgradeAdded(Upgrade upgrade)
    {
        myUpgrades.Add(upgrade.Id);
    }

    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        ClientOnInfoUpdated?.Invoke();
    }

    [ClientRpc]
    private void ClientDebug(string message)
    {
        Debug.Log(message);
    }
    #endregion
}
