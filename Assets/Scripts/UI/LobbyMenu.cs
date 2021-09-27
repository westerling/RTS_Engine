using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject lobbyUI = null;

    [SerializeField]
    private Button startGameButton = null;

    [SerializeField]
    private TMP_Text[] playerNameTexts = new TMP_Text[4];

    [SerializeField]
    private TMP_Text mapText = null;

    [SerializeField]
    private TMP_Dropdown mapDropDown = null;

    [SerializeField]
    private TMP_Dropdown factionDropDown = null;

    private readonly int minPlayers = 1;

    private void Start()
    {
        RtsNetworkManager.ClientOnConnected += HandleClientConnected;
        RtsPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
        RtsPlayer.ClientOnInfoUpdated += ClientHandleInfoUpdated;
    }

    private void OnDestroy()
    {
        RtsNetworkManager.ClientOnConnected -= HandleClientConnected;
        RtsPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
        RtsPlayer.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
    }
    
    private void ClientHandleInfoUpdated()
    {
        var players = ((RtsNetworkManager)NetworkManager.singleton).Players;

        for (var i = 0; i < players.Count; i++)
        {
            playerNameTexts[i].text = players[i].DisplayName;
            playerNameTexts[i].color = players[i].TeamColor;
        }

        for (var i = players.Count; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Open...";
        }

        startGameButton.interactable = players.Count >= minPlayers;
    }

    private void HandleClientConnected()
    {
        lobbyUI.SetActive(true);
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state)
    {
        startGameButton.gameObject.SetActive(state);
        mapDropDown.gameObject.SetActive(state);
        mapText.gameObject.SetActive(state);
    }

    public void StartGame()
    {
        var players = ((RtsNetworkManager)NetworkManager.singleton).Players;

        for (int i = 0; i < players.Count; i++)
        {
            var civ = (Faction)factionDropDown.value;
            players[i].SetFaction(civ);
        }

        NetworkClient.connection.identity.GetComponent<RtsPlayer>().CmdStartGame(mapDropDown.value);
    }

    public void LeaveLobby()
    {
        if(NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();

            SceneManager.LoadScene(0);
        }
    }
}
