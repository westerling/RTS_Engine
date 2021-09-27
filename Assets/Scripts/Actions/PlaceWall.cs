using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class PlaceWall : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int m_BuildingId = -1;

    [SerializeField]
    private int m_WallPartId = -1;

    private Building m_Building = null;
    private Building m_WallPart = null;
    private Camera m_MainCamera;
    private RtsPlayer m_Player;
    private Renderer m_BuildingRendererInstance;
    private BoxCollider m_BuildingCollider;
    private int clicks = 0;
    private Vector3 m_FirstPosition;
    private Vector3 m_SecondPosition;
    private Controls m_Controls;
    private List<GameObject> m_WallParts = new List<GameObject>();

    private void Start()
    {
        m_MainCamera = Camera.main;
        m_Player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        m_Building = m_Player.GetBuildingFromId(m_BuildingId);
        m_WallPart = m_Player.GetBuildingFromId(m_WallPartId);
        
        m_BuildingCollider = m_Building.GetComponent<BoxCollider>();
        m_BuildingRendererInstance = GetComponentInChildren<Renderer>();
        
        m_Controls = new Controls();
        m_Controls.Player.Pause.performed += GeneralControlsPerformed;
        m_Controls.Enable();
    }

    private void Update()
    {
        UpdateBuildingPreview();

        if (clicks == 1)
        {
            UpdateWall();
        }
    }

    [Client]
    public void OnPointerClick(PointerEventData eventData)
    {
        var ray = m_MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (clicks == 0)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.BuildingLayerMask))
            {
                m_Player.CmdTryPlaceBuilding(m_Building.Id, hit.point);
                m_FirstPosition = hit.point;
                clicks++;
            }
        }
        else if (clicks == 1)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.BuildingLayerMask))
            {
                var canPlace = true;

                foreach (var piece in m_WallParts)
                {
                    if (!m_Player.CanPlaceBuilding(piece.GetComponent<BoxCollider>(), piece.transform.position, m_WallPart.Id))
                    {
                        canPlace = false;
                    }
                }

                if (!canPlace)
                {
                    return;   
                }

                foreach (var piece in m_WallParts)
                {
                    m_Player.CmdTryPlaceBuilding(m_WallPart.Id, piece.transform.position);
                }
            }
            RemoveAllWallParts();
            Destroy(this.gameObject);
        }
    }

    private void RemoveAllWallParts()
    {
        foreach (var piece in m_WallParts)
        {
            Destroy(piece);
        }
    }

    private void UpdateBuildingPreview()
    {
        Ray ray = m_MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.BuildingLayerMask))
        {
            return;
        }

        transform.position = hit.point;

        var color = m_Player.CanPlaceBuilding(m_BuildingCollider, hit.point, m_Building.Id) ? Color.green : Color.red;

        m_BuildingRendererInstance.material.SetColor("_Color", color);
    }

    private void UpdateWallPreview(GameObject wallPiece)
    {
        var color = m_Player.CanPlaceBuilding(wallPiece.GetComponent<BoxCollider>(), wallPiece.transform.position, m_WallPart.Id) ? Color.green : Color.red;

        m_BuildingRendererInstance.material.SetColor("_BaseColor", color);
    }

    private void UpdateWall()
    {
        var ray = m_MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Globals.BuildingLayerMask))
        {
            return;
        }

        m_SecondPosition = hit.point;

        var distance = Vector3.Distance(m_FirstPosition, m_SecondPosition);
        var numberOfWalls = (int)distance / 5;

        while (numberOfWalls > m_WallParts.Count)
        {
            var go = Instantiate(m_WallPart.GetBuildingPreview(), Vector3.Lerp(m_FirstPosition, m_SecondPosition, 5), Quaternion.identity);

            m_WallParts.Add(go);
        }
        while (numberOfWalls < m_WallParts.Count)
        {
            Destroy(m_WallParts[m_WallParts.Count-1]);

            m_WallParts.RemoveAt(m_WallParts.Count - 1);
        }
        
        for (int i = 0; i < m_WallParts.Count; i++)
        {
            //Magcal: T=1 / (i * (wallParts.Count + 1));
            var pos = (1f / (m_WallParts.Count + 1)) * (i + 1);

            m_WallParts[i].transform.position = Vector3.Lerp(m_FirstPosition, m_SecondPosition, pos);
            m_WallParts[i].transform.LookAt(m_SecondPosition);
            UpdateWallPreview(m_WallParts[i]);
        }
    }

    private void GeneralControlsPerformed(InputAction.CallbackContext obj)
    {
        if (clicks == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        m_Controls.Player.Pause.performed -= GeneralControlsPerformed;
    }
}
