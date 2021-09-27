using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RtsNetworkManager : NetworkManager
{
    [SerializeField]
    private GameObject startBuilding = null;

    [SerializeField]
    private GameObject[] startUnits = null;

    [SerializeField]
    private GameOverHandler gameOverHandler = null;

    public List<RtsPlayer> Players { get; } = new List<RtsPlayer>();
    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    private List<Color> teamColors = new List<Color>() 
    { 
        Color.blue,
        Color.red,
        Color.green,
        Color.yellow
    };
    
    private bool gameInProgress = false;

    #region server

    public override void OnServerConnect(NetworkConnection conn)
    {
        if(!gameInProgress)
        {
            return;
        }

        conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        var player = conn.identity.GetComponent<RtsPlayer>();

        Players.Remove(player);
    }

    public override void OnStopServer()
    {
        Players.Clear();

        gameInProgress = false;
    }

    public void StartGame(int mapId)
    {
        if (Players.Count < 1)
        {
            return;
        }

        gameInProgress = true;

        ServerChangeScene("Scene_Map_01");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        var player = conn.identity.GetComponent<RtsPlayer>();

        Players.Add(player);
        player.SetTeamColor(teamColors[Players.Count-1]);
        player.SetDisplayName($"Player {Players.Count}");
        player.SetPartyOwner(Players.Count == 1);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Scene_Map"))
        {
            GameOverHandler gameOverHandlerInstance = Instantiate(gameOverHandler);

            NetworkServer.Spawn(gameOverHandlerInstance.gameObject);

            foreach(var player in Players)
            {
                var baseInstance = Instantiate(startBuilding, GetStartPosition().position, Quaternion.identity);

                NetworkServer.Spawn(baseInstance, player.connectionToClient);

                baseInstance.GetComponent<Building>().InitializeStartupBuilding();

                foreach (var unit in startUnits)
                {
                    var spawnPoint = baseInstance.GetComponent<Spawner>().SpawnPoint;

                    var unitInstance = Instantiate(unit, spawnPoint.position, Quaternion.identity);

                    NetworkServer.Spawn(unitInstance, player.connectionToClient);
                }
            }
        }
    }

    #endregion

    #region client
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        ClientOnDisconnected?.Invoke();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    #endregion




}
