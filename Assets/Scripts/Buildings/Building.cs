using Mirror;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Building : Targetable
{
    [SerializeField]
    private Health health = null;

    [SerializeField]
    private GameObject buildingPreview;

    [SerializeField]
    private GameObject construction;

    [SerializeField]
    private List<GameObject> m_EnableOnBuild = new List<GameObject>();

    [SerializeField]
    private List<Unit> builders = new List<Unit>();

    [SerializeField]
    [SyncVar(hook = nameof(HandleBuildingStateUpdated))]
    private bool m_BuildingIsCompleted = false;

    private RtsPlayer m_Player;
    private float buildTimer = 0;


    public static event Action<Building> ServerOnBuildingSpawned;
    public static event Action<Building> ServerOnBuildingDespawned;

    public static event Action<Building> AuthorityOnBuildingSpawned;
    public static event Action<Building> AuthorityOnBuildingDespawned;

    protected RtsPlayer Player
    {
        get { return m_Player; }
        set { m_Player = value; }
    }

    public bool BuildingIsCompleted
    {
        get { return m_BuildingIsCompleted; }
        set { m_BuildingIsCompleted = value; }
    }

    public List<GameObject> EnableOnBuild
    {
        get => m_EnableOnBuild;
        set => m_EnableOnBuild = value;
    }

    private void Update()
    {
        if (health.HasFullHealth())
        {
            return;
        }

        if (builders.Count <= 0)
        {
            return;
        }

        CheckBuilders();

        buildTimer += Time.deltaTime;

        if (buildTimer < 1)
        {
            return;
        }

        buildTimer = 0;

        UnitsBuild();            
    }

    private void CheckBuilders()
    {
        for (int i = builders.Count - 1; i >= 0; i--)
        {
            var task = builders[i].UnitMovement.Task;
            if (task != Task.Build)
            {
                builders.RemoveAt(i);
                continue;
            }

            var target = builders[i].Builder.Target;

            if (target != this)
            {
                builders.RemoveAt(i);
            }
        }
    }

    public void InitializeStartupBuilding()
    {
        BuildingIsCompleted = true;
    }

    public void InitializeBuilding()
    {
        SetHealth(1);
        RpcInitializeBuilding();
    }

    public bool HasBuilder(Unit builder)
    {
        return builders.Contains(builder);
    }

    public void AddBuilder(Unit unit)
    {
        if (builders.Contains(unit))
        {
            return;
        }

        builders.Add(unit);
    }

    private void UnitsBuild()
    {
        //var stats = NetworkClient.connection.identity.GetComponent<StatsManager>().GetBuildingStats(Id);
        var stats = GetComponent<LocalStats>().Stats;
        var constructionTime = 3 * (stats.GetAttributeAmount(AttributeType.Training)) / (builders.Count + 2);
        var buildPerSecond = (health.MaxHealth / constructionTime);
        var currentHealth = (health.CurrentHealth + buildPerSecond);

        if (BuildingIsCompleted)
        {
            var cost = stats.GetCost();

            IDictionary<Resource, int> repairCost = new Dictionary<Resource, int>();
            repairCost.Add(new KeyValuePair<Resource, int>(Resource.Food, cost[Resource.Food] / 2 / (int)constructionTime));
            repairCost.Add(new KeyValuePair<Resource, int>(Resource.Gold, cost[Resource.Gold] / 2 / (int)constructionTime));
            repairCost.Add(new KeyValuePair<Resource, int>(Resource.Stone, cost[Resource.Stone] / 2 / (int)constructionTime));
            repairCost.Add(new KeyValuePair<Resource, int>(Resource.Wood, cost[Resource.Wood] / 2 / (int)constructionTime));


            if (!Utils.CanAfford(Player.GetResources(), stats.GetCost()))
            {
                return;
            }

            foreach (var resource in repairCost)
            {
                Player.CmdSetResources((int)resource.Key, -resource.Value);
            }
        }

        CmdSetHealth((int)currentHealth);
    }

    public GameObject GetBuildingPreview()
    {
        return buildingPreview;
    }

    #region server

    [Command]
    private void CmdSetHealth(int amount)
    {
        health.SetHealth(amount);
    }

    [Server]
    private void SetHealth(int amount)
    {
        health.SetHealth(amount);
    }

    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;
        ServerOnBuildingSpawned?.Invoke(this);

        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
        health.EventHealthChanged += HandleHealthChanged;

        Player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
        ServerOnBuildingDespawned?.Invoke(this);

        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
        health.EventHealthChanged -= HandleHealthChanged;
    }

    [Server]
    private void ServerHandleGameOver()
    {
        enabled = false;
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }
    #endregion

    #region client


    public override void OnStartAuthority()
    {
        AuthorityOnBuildingSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!hasAuthority)
        {
            return;
        }

        AuthorityOnBuildingDespawned?.Invoke(this);
    }

    private void HandleBuildingStateUpdated(bool oldState, bool newState)
    {
        HandleEnabledComponents(newState);
    }

    public void HandleEnabledComponents(bool newState)
    {
        foreach (var go in m_EnableOnBuild)
        {
            go.SetActive(newState);
        }

        var scriptsToEnable = GetComponents<ActionBehaviour>();

        foreach (var script in scriptsToEnable)
        {
            script.enabled = newState;
        }

        construction.SetActive(!newState);
    }

    [ClientRpc]
    private void RpcInitializeBuilding()
    {
        BuildingIsCompleted = false;
        HandleEnabledComponents(false);
    }

    [ClientRpc]
    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        if (BuildingIsCompleted)
        {
            return;
        }

        if (health.HasFullHealth())
        {
            BuildingIsCompleted = true;
            m_Player.RepairableBuildings.Remove(this);
        }

        if (!m_Player.RepairableBuildings.Contains(this))
        {
            m_Player.RepairableBuildings.Add(this);
        }
    }

    public override void EnemyReaction(GameObject sender)
    {
        throw new NotImplementedException();
    }

    #endregion
}
