using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class BuildingButton : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/
{
    //[SerializeField]
    //private Building building = null;

    //[SerializeField]
    //private Image iconImage = null;

    //[SerializeField]
    //private TMP_Text text = null;

    //[SerializeField]
    //private LayerMask floorMask = new LayerMask();

    //private Camera mainCamera;
    //private RtsPlayer player;
    //private StatsManager statsManager;
    //private GameObject buildingPreviewInstance;
    //private Renderer buildingRendererInstance;
    //private BoxCollider buildingCollider;


    //private void Start()
    //{
    //    mainCamera = Camera.main;

    //    player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
    //    statsManager = NetworkClient.connection.identity.GetComponent<StatsManager>();

    //    iconImage.sprite = building.GetIcon();
    //    var stats = statsManager.GetBuildingStats(building.Id);
    //    text.text = stats.GetName(); 

    //    player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
        
    //    buildingCollider = building.GetComponent<BoxCollider>();
    //}

    //private void Update()
    //{
    //    if (buildingPreviewInstance == null)
    //    {
    //        return;
    //    }

    //    UpdateBuildingPreview();
    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    if (eventData.button != PointerEventData.InputButton.Left)
    //    {
    //        return;
    //    }

    //    //if (!Utils.CanAfford(player.GetResources(), building.GetCost()))
    //    //{
    //    //    return;
    //    //}

    //    buildingPreviewInstance = Instantiate(building.GetBuildingPreview());
    //    buildingRendererInstance = buildingPreviewInstance.GetComponentInChildren<Renderer>();

    //    buildingPreviewInstance.SetActive(false);
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    if (buildingPreviewInstance == null)
    //    {
    //        return;
    //    }

    //    Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

    //    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask))
    //    {
    //        player.CmdTryPlaceBuilding(building.Id, hit.point);
    //    }

    //    Destroy(buildingPreviewInstance);
    //}

    //public void UpdateBuildingPreview()
    //{
    //    Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

    //    if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask))
    //    {
    //        return;
    //    }

    //    buildingPreviewInstance.transform.position = hit.point;

    //    if (!buildingPreviewInstance.activeSelf)
    //    {
    //        buildingPreviewInstance.SetActive(true);
    //    }

    //    Color color = player.CanPlaceBuilding(buildingCollider, hit.point) ? Color.green : Color.red;

    //    buildingRendererInstance.material.SetColor("_Color", color);
    //}
}
