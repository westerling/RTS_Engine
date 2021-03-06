using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class Spawner : NetworkBehaviour
{
    [SerializeField]
    private Transform m_SpawnPoint;

    [SerializeField]
    private GameObject m_RallyPoint;

    [SyncVar]
    private float m_ResearchTimer;

    private List<GameObject> m_SpawnQueue = new List<GameObject>();
    private RtsPlayer player;
    private float m_SpawnTime = 0f;
    private int maxQueue = 16;
    private bool m_QueuePaused = false;

    public override void OnStartServer()
    {
        player = connectionToClient.identity.GetComponent<RtsPlayer>();
    }

    private void Update()
    {
        if (m_SpawnQueue.Count == 0)
        {
            return;
        }

        if (isServer)
        {
            ProduceFromQueue();
        }
    }

    public List<GameObject> SpawnQue
    {
        get { return m_SpawnQueue; }
        set { m_SpawnQueue = value; }
    }

    public float ResearchTimer
    {
        get { return m_ResearchTimer; }
        set { m_ResearchTimer = value; }
    }

    public Transform SpawnPoint
    {
        get { return m_SpawnPoint; }
        set { m_SpawnPoint = value; }
    }

    public GameObject RallyPoint
    {
        get { return m_RallyPoint; }
        set { m_RallyPoint = value; }
    }

    public float SpawnTime
    {
        get { return m_SpawnTime; }
        set { m_SpawnTime = value; }
    }

    #region server

    [Server]
    private void ProduceFromQueue()
    {
        ResearchTimer += Time.deltaTime;

        var objectToSpawn = m_SpawnQueue[0];
        
        if (objectToSpawn == null)
        {
            return;
        }

        if (SpawnTime == 0)
        {
            SetSpawnTime();
        }

        if (ResearchTimer < SpawnTime)
        {
            return;
        }

        if (objectToSpawn.TryGetComponent(out Unit unit))
        {
            if (player.CurrentPopulation >= player.MaxPopulation)
            {
                ResearchTimer = SpawnTime;
                return;
            }


            var spawnInstance = Instantiate(objectToSpawn, SpawnPoint.position, SpawnPoint.rotation);
            var unitMovement = spawnInstance.GetComponent<UnitMovement>();
            spawnInstance.GetComponent<Targetable>().SetFOVAvailability(true);

            NetworkServer.Spawn(spawnInstance, connectionToClient);
            unitMovement.ServerMove(RallyPoint.transform.position);
        }
        else if (objectToSpawn.TryGetComponent(out Upgrade upgrade))
        {
            var spawnInstance = Instantiate(objectToSpawn, SpawnPoint.position, SpawnPoint.rotation);
            NetworkServer.Spawn(spawnInstance, connectionToClient);
        }

        SpawnQue.RemoveAt(0);
        ResearchTimer = 0;
        SpawnTime = 0;
    }

    [Server]
    private void SetSpawnTime()
    {
        var objectToSpawn = m_SpawnQueue[0];

        if (objectToSpawn.TryGetComponent(out Unit unit))
        {
            var stats = unit.GetComponent<LocalStats>().Stats;

            SpawnTime = stats.GetAttributeAmount(AttributeType.Training);
        }
        else if (objectToSpawn.TryGetComponent(out Upgrade upgrade))
        {
            var stats = upgrade.GetComponent<LocalStats>().Stats;

            SpawnTime = stats.GetAttributeAmount(AttributeType.Training); ;
        }
    }

    [Command]
    public void CmdAddToQueue(int id)
    {
        if (m_SpawnQueue.Count >= maxQueue)
        {
            return;
        }
      
        var go = player.GetGameobjectFromId(id);

        if (go.TryGetComponent(out Unit unit))
        {
            m_SpawnQueue.Add(go);
        }
        else if (go.TryGetComponent(out Upgrade upgrade))
        {
            m_SpawnQueue.Add(go);
        }
    }

    [Command]
    public void CmdRemoveFromQueue(int id)
    {
        SpawnQue.RemoveAt(id);
    }

    #endregion
}
