using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlaceBuilding : PlaceBuildingBase
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        var ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.TerrainLayerMask))
        {
            Player.CmdTryPlaceBuilding(Building.Id, hit.point, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }

    public override void UpdateBuildingPreview()
    {
        Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.TerrainLayerMask))
        {
            return;
        }

        transform.position = hit.point;

        var color = Player.CanPlaceBuilding(BuildingCollider, hit.point) ? Color.green : Color.red;

        BuildingRendererInstance.material.SetColor("_Color", color);
    }
}
