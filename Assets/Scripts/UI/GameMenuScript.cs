using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenuScript : NetworkBehaviour
{
    [SerializeField]
    private GameObject m_MainMenu = null;
    
    private Controls m_Controls;

    void Start()
    {
        m_Controls = new Controls();
        m_Controls.Player.Pause.performed += GeneralControlsPerformed;
        m_Controls.Enable();
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
        m_Controls.Player.Pause.performed -= GeneralControlsPerformed;
    }
}
