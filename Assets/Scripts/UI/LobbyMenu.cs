using TMPro;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LobbyMenu : NetworkBehaviour
{
    [SerializeField]
    private GameObject m_LobbyUI = null;

    [SerializeField]
    private Button m_StartGameButton = null;

    [SerializeField]
    private TMP_Text[] m_PlayerNameTexts = new TMP_Text[4];

    [SerializeField]
    private TMP_Text m_MapText = null;

    [SerializeField]
    private TMP_Dropdown m_MapDropDown = null;

    [SerializeField]
    private TMP_Dropdown m_FactionDropDown = null;

    private readonly int m_MinPlayers = 1;

    private void Start()
    {
        m_FactionDropDown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_FactionDropDown);
        });

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
            m_PlayerNameTexts[i].text = players[i].DisplayName;
            m_PlayerNameTexts[i].color = players[i].TeamColor;
        }

        for (var i = players.Count; i < m_PlayerNameTexts.Length; i++)
        {
            m_PlayerNameTexts[i].text = "Open...";
        }

        m_StartGameButton.interactable = players.Count >= m_MinPlayers;
    }

    private void HandleClientConnected()
    {
        m_LobbyUI.SetActive(true);
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state)
    {
        m_StartGameButton.gameObject.SetActive(state);
        m_MapDropDown.gameObject.SetActive(state);
        m_MapText.gameObject.SetActive(state);
    }

    public void StartGame()
    {
        var players = ((RtsNetworkManager)NetworkManager.singleton).Players;

        for (int i = 0; i < players.Count; i++)
        {
            var civ = (Faction)m_FactionDropDown.value;
            players[i].SetFaction(civ);
        }

        NetworkClient.connection.identity.GetComponent<RtsPlayer>().CmdStartGame(m_MapDropDown.value);
    }

    public void DropdownValueChanged(TMP_Dropdown change)
    {
        var player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
        player.CmdSetFaction(change.value);
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
