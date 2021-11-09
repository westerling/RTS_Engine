using Mirror;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Building : Targetable
{
    [SerializeField]
    private Health m_Health = null;

    [SerializeField]
    private GameObject m_BuildingPreview;

    [SerializeField]
    private GameObject m_Construction;

    [SerializeField]
    private List<GameObject> m_EnableOnBuild = new List<GameObject>();

    [SerializeField]
    [SyncVar(hook = nameof(HandleBuildingStateUpdated))]
    private bool m_BuildingIsCompleted = false;

    [SerializeField]
    private GameObject[] m_DestructionStageOne;

    [SerializeField]
    private GameObject[] m_DestructionStageTwo;

    [SerializeField]
    private bool m_CanRotate;

    private List<Unit> m_Builders = new List<Unit>();
    private RtsPlayer m_Player;
    private float m_BuildTimer = 0;

    public static event Action<Building> ServerOnConstructionStarted;
    public static event Action<Building> ServerOnBuildingCompleted;
    public static event Action<Building> ServerOnBuildingDespawned;

    public static event Action<Building> AuthorityOnConstructionStarted;
    public static event Action<Building> AuthorityOnBuildingCompleted;
    public static event Action<Building> AuthorityOnBuildingDespawned;

    protected RtsPlayer Player
    {
        get => m_Player;
        set => m_Player = value;
    }

    public bool BuildingIsCompleted
    {
        get => m_BuildingIsCompleted;
        set => m_BuildingIsCompleted = value;
    }

    public List<GameObject> EnableOnBuild
    {
        get => m_EnableOnBuild;
    }

    public bool CanRotate 
    { 
        get => m_CanRotate; 
    }   
    
    public GameObject BuildingPreview
    {
        get => m_BuildingPreview;
    }


    private void Update()
    {
        if (m_Health.HasFullHealth())
        {
            return;
        }

        if (m_Builders.Count <= 0)
        {
            return;
        }

        CheckBuilders();

        m_BuildTimer += Time.deltaTime;

        if (m_BuildTimer < 1)
        {
            return;
        }

        m_BuildTimer = 0;

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

    public void InitializeConstruction()
    {
        m_Health.SetHealth(1);
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
        var buildPerSecond = (m_Health.MaxHealth / constructionTime);
        var currentHealth = (m_Health.CurrentHealth + buildPerSecond);

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

        m_Health.CmdSetHealth((int)currentHealth);
    }

    #region server

    public override void OnStartServer()
    {
        m_Health.ServerOnDie += ServerHandleDie;

        ServerOnConstructionStarted?.Invoke(this);

        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
        StartupBuilding.ServerOnStartupBuildingAdded += ServerHandleStartBuilding;
        m_Health.EventHealthChanged += RpcHandleHealthChanged;

        Player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
    }

    public override void OnStopServer()
    {
        m_Health.ServerOnDie -= ServerHandleDie;
        ServerOnBuildingDespawned?.Invoke(this);

        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
        StartupBuilding.ServerOnStartupBuildingAdded -= ServerHandleStartBuilding;
        m_Health.EventHealthChanged -= RpcHandleHealthChanged;
    }

    [Command]
    private void CmdBuildingCompleted()
    {
        ServerOnBuildingCompleted?.Invoke(this);
    }

    [Server]
    private void ServerHandleStartBuilding()
    {
        m_BuildingIsCompleted = true;
        ServerOnBuildingCompleted?.Invoke(this);
        HandleEnabledComponents(true);
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
        StartupBuilding.AuthorityOnStartupBuildingAdded += AuthorityHandleStartBuilding;
    }

    private void AuthorityHandleStartBuilding()
    {
        if (hasAuthority)
        {
            m_BuildingIsCompleted = true;
            AuthorityOnBuildingCompleted?.Invoke(this);
            HandleEnabledComponents(true);
        }
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

        m_Construction.SetActive(!newState);

        if (hasAuthority)
        {
            SetFOVAvailability(newState);
        }
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

        if (m_Health.HasFullHealth())
        {
            BuildingIsCompleted = true;
            
            if (hasAuthority)
            {
                Debug.Log("shits done");
                AuthorityOnBuildingCompleted?.Invoke(this);
                CmdBuildingCompleted();
            }

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

    [ClientRpc]
    private void ClientDebug(string message)
    {
        Debug.Log(message);
    }
    #endregion
}
