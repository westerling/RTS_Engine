using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInput m_Controls;

    public static InputManager Current;

    public PlayerInput Controls
    { 
        get => m_Controls; 
        set => m_Controls = value; 
    }

    public InputManager()
    {
        Current = this;
    }

    public void SetContext(GameContext context)
    {
        var actionMap = string.Empty;

        switch (context)
        {
            case GameContext.Camera:
                actionMap = "Camera";
                break;
            case GameContext.Normal:
                actionMap = "Normal";
                break;
            case GameContext.Selected:
                actionMap = "Selected";
                break;
            case GameContext.Build:
                actionMap = "Build";
                break;
            case GameContext.Menu:
                actionMap = "Menu";
                break;
        }

        Controls.SwitchCurrentActionMap(actionMap);
    }
}
