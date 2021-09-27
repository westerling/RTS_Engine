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
    private int food;

    [SyncVar(hook = nameof(ClientHandleGoldUpdated))]
    private int gold;

    [SyncVar(hook = nameof(ClientHandleStoneUpdated))]
    private int stone;

    [SyncVar(hook = nameof(ClientHandleWoodUpdated))]
    private int wood;

    [SyncVar(hook = nameof(ClientHandleCurrentPopulationUpdated))]
    private int m_CurrentPopulation;

    [SyncVar(hook = nameof(ClientHandleMaximumPopulationUpdated))]
    private int m_MaxPopulation;

    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    private bool isPartyOwner = false;

    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    private string m_DisplayName;

    [SerializeField]
    private List<int> myUpgrades = new List<int> {0};

    private Faction m_Faction;
    public event Action<int, Resource> ClientOnResourcesUpdated;
    public event Action<int> ClientOnCurrentPopulationUpdated;
    public event Action<int> ClientOnMaximumPopulationUpdated;
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;
    public static event Action ClientOnInfoUpdated;

    private List<GameObjectIdentity> m_Researchables = new List<GameObjectIdentity>();
    private List<Unit> m_DeployedUnits = new List<Unit>();
    private List<Building> m_DeployedBuildings = new List<Building>();
    private List<Building> m_RepairableBuldings = new List<Building>();
    private Color m_TeamColor = new Color();

    public int CurrentPopulation
    {
        get { return m_CurrentPopulation; }
    }

    public int MaxPopulation
    {
        get { return m_MaxPopulation; }
    }

    public string DisplayName
    {
        get { return m_DisplayName; }
        set { m_DisplayName = value; }
    }

    public Transform MainCameraTransform
    {
        get { return m_MainCameraTransform; }
        set { m_MainCameraTransform = value; }
    }

    public Color TeamColor
    {
        get { return m_TeamColor; }
        set { m_TeamColor = value; }
    }

    public Faction Faction
    {
        get { return m_Faction; }
        set { m_Faction = value; }
    }

    public List<Building> RepairableBuildings
    {
        get { return m_RepairableBuldings; }
        set { m_RepairableBuldings = value; }
    }

    public List<Unit> GetMyUnits()
    {
        return m_DeployedUnits;
    }

    public List<int> GetMyUpgrades()
    {
        return myUpgrades;
    }

    public List<Building> GetMyBuildings()
    {
        return m_DeployedBuildings;
    }

    public bool GetPartyOwner()
    {
        return isPartyOwner;
    }

    public IDictionary<Resource, int> GetResources()
    {
        IDictionary<Resource, int> d = new Dictionary<Resource, int>();
        d.Add(new KeyValuePair<Resource, int>(Resource.Food, food));
        d.Add(new KeyValuePair<Resource, int>(Resource.Gold, gold));
        d.Add(new KeyValuePair<Resource, int>(Resource.Stone, stone));
        d.Add(new KeyValuePair<Resource, int>(Resource.Wood, wood));

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
        var stats = building.GetLocalStats();
        var cost = stats.GetCost();
        var resources = GetResources();


        if (!Utils.CanAfford(resources, cost))
        {
            return false;
        }

        return true;
    }

    public bool CanPlaceBuilding(BoxCollider collider, Vector3 point, int id)
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
        Unit.ServerOnUnitDespawned += ServerHandleUnitsSpawned;

        Building.ServerOnBuildingSpawned += ServerHandleBuildingsSpawned;
        Building.ServerOnBuildingDespawned += ServerHandleBuildingsDespawned;

        Upgrade.ServerOnUpgradeAdded += ServerHandleUpgradeAdded;

        SetStartResources();

        SetupResearchables();

        DontDestroyOnLoad(gameObject);
    }

    private void SetStartResources()
    {
        food = 5000;
        gold = 5000;
        stone = 5000;
        wood = 5000;
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
        Unit.ServerOnUnitDespawned -= ServerHandleUnitsSpawned;

        Building.ServerOnBuildingSpawned -= ServerHandleBuildingsSpawned;
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
        isPartyOwner = state;
    }

    [Server]
    public void SetTeamColor(Color newTeamColor)
    {
        m_TeamColor = newTeamColor;
    }

    [Server]
    public void SetFaction(Faction newFaction)
    {
        Faction = newFaction;
    }

    [Command]
    public void CmdSetMaximumPopulation(int population)
    {
        m_MaxPopulation += population;
    }

    [Server]
    public void SetMaximumPopulation(int population)
    {
        m_MaxPopulation += population;
    }

    [Command]
    public void CmdSetCurrentPopulation(int population)
    {
        m_CurrentPopulation += population;
    }

    [Server]
    public void SetCurrentPopulation(int population)
    {
        m_CurrentPopulation += population;
    }

    [Command]
    public void CmdSetResources(int resourceType, int amount)
    {
        switch ((Resource)resourceType)
        {
            case Resource.Food:
                food += amount;
                break;
            case Resource.Gold:
                gold += amount;
                break;
            case Resource.Stone:
                stone += amount;
                break;
            case Resource.Wood:
                wood += amount;
                break;
        }
    }

    [Server]
    public void SetResources(int resourceType, int amount)
    {
        switch ((Resource)resourceType)
        {
            case Resource.Food:
                food += amount;
                break;
            case Resource.Gold:
                gold += amount;
                break;
            case Resource.Stone:
                stone += amount;
                break;
            case Resource.Wood:
                wood += amount;
                break;
        }
    }

    [Command]
    public void CmdStartGame(int mapId)
    {
        if (!isPartyOwner)
        {
            return;
        }

        ((RtsNetworkManager)NetworkManager.singleton).StartGame(mapId);
    }

    [Command]
    public void CmdTryPlaceBuilding(int buildingId, Vector3 point)
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

        if (!CanPlaceBuilding(collider, point, buildingId))
        {
            return;
        }

        var buildingInstance = Instantiate(
            buildingToPlace.gameObject, 
            point, 
            buildingToPlace.transform.rotation);

        NetworkServer.Spawn(buildingInstance, connectionToClient);

        //var stats = m_StatsManager.GetBuildingStats(buildingId);
        var stats = buildingToPlace.GetLocalStats();
        var cost = stats.GetCost();

        foreach (var resourceCostItem in cost)
        {
            CmdSetResources((int)resourceCostItem.Key, -cost[resourceCostItem.Key]);
        }

        buildingInstance.GetComponent<Building>().InitializeBuilding();
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

        m_DeployedUnits.Add(unit);
        SetCurrentPopulation(1);
        
        if (!hasAuthority)
        {
            return;
        }
        
        unit.SetFOVAvailability(true);
    }    
    
    private void ServerHandleUnitsDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        m_DeployedUnits.Remove(unit);
        SetCurrentPopulation(-1);
    } 
    
    private void ServerHandleBuildingsSpawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        m_DeployedBuildings.Add(building);
        building.SetFOVAvailability(true);

        //var stats = m_StatsManager.GetBuildingStats(building.Id);
        var stats = building.GetLocalStats();
        SetMaximumPopulation((int)stats.GetAttributeAmount(AttributeType.Population));
    }    
    
    private void ServerHandleBuildingsDespawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        m_DeployedBuildings.Remove(building);

        var stats = building.GetLocalStats();
        SetMaximumPopulation(-(int)stats.GetAttributeAmount(AttributeType.Population));
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

        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitsSpawned;
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitsDespawned;

        Building.AuthorityOnBuildingSpawned += AuthorityHandleBuildingsSpawned;
        Building.AuthorityOnBuildingDespawned += AuthorityHandleBuildingsDespawned;

        Upgrade.AuthorityOnUpgradeAdded += AuthorityHandleUpgradeAdded;
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

        Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitsSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitsDespawned;

        Building.AuthorityOnBuildingSpawned -= AuthorityHandleBuildingsSpawned;
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

    private void AuthorityHandleUnitsSpawned(Unit unit)
    {
        m_DeployedUnits.Add(unit);
        CmdSetCurrentPopulation(1);
        unit.SetFOVAvailability(true);
    }

    private void AuthorityHandleUnitsDespawned(Unit unit)
    {
        m_DeployedUnits.Remove(unit);
        CmdSetCurrentPopulation(-1);
    }

    private void AuthorityHandleBuildingsSpawned(Building building)
    {
        m_DeployedBuildings.Add(building);
        building.SetFOVAvailability(true);
    }

    private void AuthorityHandleBuildingsDespawned(Building building)
    {
        m_DeployedBuildings.Remove(building);

        var stats = building.GetLocalStats();
        CmdSetMaximumPopulation(-(int)stats.GetAttributeAmount(AttributeType.Population));
    }

    private void AuthorityHandleUpgradeAdded(Upgrade upgrade)
    {
        myUpgrades.Add(upgrade.Id);

        //UpgradeCheck(upgrade);
    }

    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        ClientOnInfoUpdated?.Invoke();
    }
    #endregion
}
