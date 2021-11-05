using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class PlaceWall : PlaceBuildingBase
{
    [SerializeField]
    private Building m_WallPart = null;

    [SerializeField]
    private GameObject m_DummyTower = null;

    private int m_Clicks = 0;
    private Vector3 m_FirstPosition;
    private Vector3 m_SecondPosition;
    private List<GameObject> m_WallParts = new List<GameObject>();
    private List<GameObject> m_Towers = new List<GameObject>();
    private List<GameObject> m_DisableColliderList = new List<GameObject>();

    private void RemoveAllWallParts()
    {
        foreach (var piece in m_WallParts)
        {
            Destroy(piece);
        }
        foreach (var tower in m_Towers)
        {
            Destroy(tower);
        }
    }

    private void UpdateWallPreview(GameObject wallPiece)
    {
        var color = Player.CanPlaceBuilding(wallPiece.GetComponent<BoxCollider>(), wallPiece.transform.position) ? Color.green : Color.red;

        BuildingRendererInstance.material.SetColor("_BaseColor", color);
    }

    private void UpdateWall()
    {
        var ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.TerrainLayerMask))
        {
            return;
        }

        m_SecondPosition = hit.point;

        var distance = Vector3.Distance(m_FirstPosition, m_SecondPosition);
        var numberOfWalls = (int)distance / 5;


        while (numberOfWalls > m_WallParts.Count)
        {
            var go = Instantiate(m_WallPart.BuildingPreview, Vector3.Lerp(m_FirstPosition, m_SecondPosition, 5), Quaternion.identity);

            m_WallParts.Add(go);
        }
        while (numberOfWalls < m_WallParts.Count)
        {
            Destroy(m_WallParts[m_WallParts.Count-1]);

            m_WallParts.RemoveAt(m_WallParts.Count - 1);
        }
        
        for (int i = 0; i < m_WallParts.Count; i++)
        {
            var pos = (1f / (m_WallParts.Count + 1)) * (i + 1);

            m_WallParts[i].transform.position = Vector3.Lerp(m_FirstPosition, m_SecondPosition, pos);
            m_WallParts[i].transform.LookAt(m_SecondPosition);
            UpdateWallPreview(m_WallParts[i]);
        }
    }

    public override void GeneralControlsPerformed(InputAction.CallbackContext obj)
    {
        if (m_Clicks == 0)
        {
            Destroy(this.gameObject);
        }
    }

    public override void UpdateBuildingPreview()
    {
        Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.WallPlacementLayerMask))
        {
            return;
        }

        transform.position = hit.point;
        if (hit.collider.TryGetComponent(out Tower tower))
        {
            BuildingRendererInstance.material.SetColor("_Color", Color.green);
        }
        else
        {
            var color = Player.CanPlaceBuilding(BuildingCollider, hit.point) ? Color.green : Color.red;

            BuildingRendererInstance.material.SetColor("_Color", color);
        }

        if (m_Clicks == 1)
        {
            UpdateWall();
        }
    }

    [Client]
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RemoveAll();
            return;
        }

        var ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        var skipFirst = false;
        var SkipEnd = false;

        if (m_Clicks == 0)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.WallPlacementLayerMask))
            {
                if (hit.collider.TryGetComponent(out Building building))
                {
                    m_FirstPosition = building.transform.position;
                    m_DisableColliderList.Add(building.gameObject);
                }
                else
                {
                    m_FirstPosition = hit.point;
                    var go = Instantiate(m_DummyTower, hit.point, Quaternion.identity);
                    m_Towers.Add(go);
                }
              
                m_Clicks++;
            }
        }
        else if (m_Clicks == 1)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.WallPlacementLayerMask))
            {
                if (hit.collider.TryGetComponent(out Building building))
                {
                    SkipEnd = true;
                    m_SecondPosition = building.transform.position;
                    m_DisableColliderList.Add(building.gameObject);
                }
                else
                {
                    m_SecondPosition = hit.point;
                }

                var canPlace = true;

                foreach (var piece in m_WallParts)
                {
                    if (!Player.CanPlaceBuilding(piece.GetComponent<BoxCollider>(), piece.transform.position))
                    {
                        canPlace = false;
                    }
                }

                if (!canPlace)
                {
                    return;
                }

                AlterCollider(m_DisableColliderList, false);

                foreach (var piece in m_WallParts)
                {
                    Player.CmdTryPlaceBuilding(m_WallPart.Id, piece.transform.position, piece.transform.rotation);
                }

                if (!skipFirst)
                {
                    Player.CmdTryPlaceBuilding(Building.Id, m_FirstPosition, Building.transform.rotation);
                }

                if (!SkipEnd)
                {
                    Player.CmdTryPlaceBuilding(Building.Id, m_SecondPosition, Building.transform.rotation);
                }

                AlterCollider(m_DisableColliderList, true);
            }

            RemoveAll();
        }
    }

    private void AlterCollider(List<GameObject> alterColliderList, bool enabled)
    {
        foreach (var go in alterColliderList)
        {
            go.GetComponent<Collider>().enabled = enabled;
        }
    }

    private void RemoveAll()
    {
        RemoveAllWallParts();
        Destroy(this.gameObject);
    }
}
