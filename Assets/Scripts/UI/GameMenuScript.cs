using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenuScript : NetworkBehaviour
{
    [SerializeField]
    private GameObject m_MainMenu = null;

    [SerializeField]
    private List<GameObject> m_DisableOnMenu = new List<GameObject>();

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
        ToggleOnMenu(false);
    }

    private void ResumePerformed(InputAction.CallbackContext obj)
    {
        InputManager.Current.SetContext(GameContext.Normal);
        m_MainMenu.SetActive(false);
        ToggleOnMenu(true);
    }

    private void ToggleOnMenu(bool enabled)
    {
        foreach (var go in m_DisableOnMenu)
        {
            go.SetActive(enabled);
        }
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
        if (InputManager.Current == null)
        {
            return;
        }

        InputManager.Current.Controls.actions[Pause].performed -= PausePerformed;
        InputManager.Current.Controls.actions[Resume].performed -= ResumePerformed;
    }
}
