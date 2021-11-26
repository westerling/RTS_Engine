using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenuScript : NetworkBehaviour
{
    [SerializeField]
    private GameObject m_MainMenu = null;

    private const string Pause = "Pause";
    private const string Resume = "Resume";

    void Start()
    {
        InputManager.Current.Controls.actions[Pause].performed += PausePerformed;
        InputManager.Current.Controls.actions[Resume].performed += ResumePerformed;
    }

    private void PausePerformed(InputAction.CallbackContext obj)
    {
        InputManager.Current.SetContext(GameContext.Menu);
        m_MainMenu.SetActive(true);
    }

    private void ResumePerformed(InputAction.CallbackContext obj)
    {
        InputManager.Current.SetContext(GameContext.Normal);
        m_MainMenu.SetActive(false);
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
        InputManager.Current.Controls.actions[Pause].performed -= PausePerformed;
        InputManager.Current.Controls.actions[Resume].performed -= ResumePerformed;
    }
}
