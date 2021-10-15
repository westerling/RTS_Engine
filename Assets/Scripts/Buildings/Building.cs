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
    private List<Unit> m_Builders = new List<Unit>();

    [SerializeField]
    [SyncVar(hook = nameof(HandleBuildingStateUpdated))]
    private bool m_BuildingIsCompleted = false;

    [SerializeField]
    private GameObject[] m_DestructionStageOne;

    [SerializeField]
    private GameObject[] m_DestructionStageTwo;

    [SerializeField]
    private bool m_CanRotate;

    private RtsPlayer m_Player;
    private float buildTimer = 0;


    public static event Action<Building> ServerHandleConstructionStarted;
    public static event Action<Building> ServerHandleBuildingCompleted;
    public static event Action<Building> ServerOnBuildingDespawned;

    public static event Action<Building> AuthorityOnConstructionStarted;
    public static event Action<Building> AuthorityOnBuildingCompleted;
    public static event Action<Building> AuthorityOnBuildingDespawned;

    protected RtsPlayer Player
    {
        get { return m_Player; }
        set { m_Player = value; }
    }

    public bool BuildingIsCompleted
    {
        get => m_BuildingIsCompleted;
        set 
        { 
            if (value)
            {
                if (hasAuthority)
                {
                    AuthorityOnBuildingCompleted?.Invoke(this);
                }
                ServerHandleBuildingCompleted?.Invoke(this);
            }

            m_BuildingIsCompleted = value;
        }
    }

    public List<GameObject> EnableOnBuild
    {
        get => m_EnableOnBuild;
        set => m_EnableOnBuild = value;
    }

    public bool CanRotate 
    { 
        get => m_CanRotate; 
        set => m_CanRotate = value; 
    }

    private void Update()
    {
        if (health.HasFullHealth())
        {
            return;
        }

        if (m_Builders.Count <= 0)
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
        for (int i = m_Builders.Count - 1; i >= 0; i--)
        {
            var task = m_Builders[i].UnitMovement.Task;
            if (task != Task.Build)
            {
                m_Builders.RemoveAt(i);
                continue;
            }

            var target = m_Builders[i].Builder.Target;

            if (target != this)
            {
                m_Builders.RemoveAt(i);
            }
        }
    }

    public void InitializeStartupBuilding()
    {
        AuthorityOnBuildingCompleted?.Invoke(this);
        BuildingIsCompleted = true;
    }

    public void InitializeBuilding()
    {
        SetHealth(1);
        RpcInitializeBuilding();
    }

    public bool HasBuilder(Unit builder)
    {
        return m_Builders.Contains(builder);
    }

    public void AddBuilder(Unit unit)
    {
        if (m_Builders.Contains(unit))
        {
            return;
        }

        m_Builders.Add(unit);
    }

    private void UnitsBuild()
    {
        var stats = GetComponent<LocalStats>().Stats;
        var constructionTime = 3 * (stats.GetAttributeAmount(AttributeType.Training)) / (m_Builders.Count + 2);
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
        ServerHandleConstructionStarted?.Invoke(this);

        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
        health.EventHealthChanged += RpcHandleHealthChanged;

        Player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
        ServerOnBuildingDespawned?.Invoke(this);

        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
        health.EventHealthChanged -= RpcHandleHealthChanged;
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
        AuthorityOnConstructionStarted?.Invoke(this);
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
        GetComponent<Collider>().enabled = true;

        foreach (var go in EnableOnBuild)
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
    private void RpcHandleHealthChanged(int currentHealth, int maxHealth)
    {
        if (BuildingIsCompleted)
        {
            CheckDamage((float)currentHealth/ maxHealth);
            return;
        }

        if (health.HasFullHealth())
        {
            BuildingIsCompleted = true;

            foreach (var builder in m_Builders)
            {
                builder.GetComponent<Builder>().FindNewTarget();
            }
        }
    }

    private void CheckDamage(float percentage)
    {
        if (percentage >= 0.75)
        {
            ChangeDestructionStageOneState(false);
            ChangeDestructionStageTwoState(false);
        }
        else if (percentage < 0.75 && percentage >= 0.50)
        {
            ChangeDestructionStageOneState(true);
            ChangeDestructionStageTwoState(false);
        }
        else if (percentage < 0.50)
        {
            ChangeDestructionStageOneState(true);
            ChangeDestructionStageTwoState(true);
        }
    }

    private void ChangeDestructionStageOneState(bool enabled)
    {
        foreach (var fire in m_DestructionStageOne)
        {
            fire.SetActive(enabled);
        }
    }

    private void ChangeDestructionStageTwoState(bool enabled)
    {
        foreach (var fire in m_DestructionStageTwo)
        {
            fire.SetActive(enabled);
        }
    }

    public override void EnemyReaction(GameObject sender)
    {
    }

    #endregion
}
