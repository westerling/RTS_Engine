using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public abstract class PlaceBuildingBase : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Building m_Building = null;

    private Camera m_MainCamera;
    private RtsPlayer m_Player;
    private Renderer m_BuildingRendererInstance;
    private BoxCollider m_BuildingCollider;
    private Controls m_Controls;

    private const string Exit = "Stop";
    private const string Scroll = "MouseScrollY";

    public Camera MainCamera 
    { 
        get => m_MainCamera; 
        set => m_MainCamera = value; 
    }

    public RtsPlayer Player
    { 
        get => m_Player; 
        set => m_Player = value; 
    }

    public Renderer BuildingRendererInstance 
    { 
        get => m_BuildingRendererInstance; 
        set => m_BuildingRendererInstance = value; 
    }

    public BoxCollider BuildingCollider
    { 
        get => m_BuildingCollider;
        set => m_BuildingCollider = value; 
    }

    public Controls Controls
    { 
        get => m_Controls; 
        set => m_Controls = value;
    }

    public Building Building
    {
        get => m_Building; 
        set => m_Building = value; 
    }

    void Start()
    {
        MainCamera = Camera.main;
        Player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        BuildingCollider = Building.GetComponent<BoxCollider>();
        BuildingRendererInstance = GetComponentInChildren<Renderer>();
       
        InputManager.Current.Controls.actions[Exit].performed += GeneralControlsPerformed;
        InputManager.Current.Controls.actions[Scroll].performed += ScrollPerformed;
    }

    private void ScrollPerformed(InputAction.CallbackContext obj)
    {
        if (!Building.CanRotate)
        {
            return;
        }

        var rotation = obj.ReadValue<float>();

        if (rotation > 0)
        {
            transform.Rotate(0, 45, 0);
        }
        else if (rotation < 0)
        {
            transform.Rotate(0, -45, 0);
        }
    }

    void Update()
    {
        UpdateBuildingPreview();
    }

    public abstract void UpdateBuildingPreview();

    public abstract void OnPointerClick(PointerEventData eventData);

    public virtual void GeneralControlsPerformed(InputAction.CallbackContext obj)
    {
        InputManager.Current.SetContext(GameContext.Normal);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        InputManager.Current.Controls.actions[Exit].performed -= GeneralControlsPerformed;
        InputManager.Current.Controls.actions[Scroll].performed -= ScrollPerformed;
    }
}
