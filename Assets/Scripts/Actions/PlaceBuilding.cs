using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine;

public class PlaceBuilding : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Building building = null;

    private Camera mainCamera;
    private RtsPlayer player;
    private Renderer buildingRendererInstance;
    private BoxCollider buildingCollider;
    private Controls m_Controls;

    private void Start()
    {
        mainCamera = Camera.main;
        player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        buildingCollider = building.GetComponent<BoxCollider>();
        buildingRendererInstance = GetComponentInChildren<Renderer>();

        m_Controls = new Controls();
        m_Controls.Player.Pause.performed += GeneralControlsPerformed;
        m_Controls.Enable();
    }

    private void Update()
    {
        UpdateBuildingPreview();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.BuildingLayerMask))
        {
            player.CmdTryPlaceBuilding(building.Id, hit.point);
        }

        Destroy(this.gameObject);
    }

    public void UpdateBuildingPreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.BuildingLayerMask))
        {
            return;
        }

        transform.position = hit.point;

        var color = player.CanPlaceBuilding(buildingCollider, hit.point, building.Id) ? Color.green : Color.red;

        buildingRendererInstance.material.SetColor("_Color", color);
    }

    private void GeneralControlsPerformed(InputAction.CallbackContext obj)
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        m_Controls.Player.Pause.performed -= GeneralControlsPerformed;
    }
}
