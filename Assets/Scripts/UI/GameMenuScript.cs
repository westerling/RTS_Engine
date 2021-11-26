using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenuScript : NetworkBehaviour
{
    [SerializeField]
    private GameObject m_MainMenu = null;

    void Start()
    {
        InputManager.Current.Controls.actions["Pause"].performed += GeneralControlsPerformed;
    }

    private void GeneralControlsPerformed(InputAction.CallbackContext obj)
    {
        m_MainMenu.SetActive(!m_MainMenu.activeInHierarchy);
    }

    public void LeaveGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }

        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        InputManager.Current.Controls.actions["Pause"].performed -= GeneralControlsPerformed;
    }
}
