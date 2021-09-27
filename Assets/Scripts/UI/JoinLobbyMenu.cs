using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject landingPagePanel = null;

    [SerializeField]
    private TMP_InputField adressInput = null;

    [SerializeField]
    private Button joinButton = null;

    private void OnEnable()
    {
        RtsNetworkManager.ClientOnConnected += HandleClientConnected;
        RtsNetworkManager.ClientOnDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        RtsNetworkManager.ClientOnConnected -= HandleClientConnected;
        RtsNetworkManager.ClientOnDisconnected -= HandleClientDisconnected;
    }

    public void Join()
    {
        var adress = adressInput.text;

        NetworkManager.singleton.networkAddress = adress;
        NetworkManager.singleton.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
