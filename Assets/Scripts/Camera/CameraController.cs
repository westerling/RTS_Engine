using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : NetworkBehaviour
{
    [SerializeField]
    private Transform playerCameraTransform = null;

    [SerializeField]
    private Vector2 screenXLimit = Vector2.zero;
    
    [SerializeField]
    private Vector2 screenZLimit = Vector2.zero;

    [SerializeField]
    private Transform groundTarget = null;

    [SerializeField]
    private LayerMask groundTargetLayerMask = new LayerMask();

    [SerializeField]
    private Material m_MinimapIndicatorMaterial;

    [SerializeField]
    private int m_CameraHeight = 100;

    private float speed = 50f;
    private float screenBorderThickness = 10f;
    private float m_MinimapIndicatorStrokeWidth = 0.1f;
    private Vector2 previousInput;
    private Transform m_MinimapIndicator;
    private Mesh m_MinimapIndicatorMesh;

    private const string CameraMovement = "Move Camera";

    public override void OnStartAuthority()
    {
        playerCameraTransform.gameObject.SetActive(true);

        InputManager.Current.Controls.actions[CameraMovement].performed += SetPreviousInput;
        InputManager.Current.Controls.actions[CameraMovement].canceled += SetPreviousInput;

        groundTarget.position = Utils.MiddleOfScreenPointToWorld(groundTargetLayerMask);
    }

    public override void OnStopAuthority()
    {
        InputManager.Current.Controls.actions[CameraMovement].performed -= SetPreviousInput;
        InputManager.Current.Controls.actions[CameraMovement].canceled -= SetPreviousInput;
    }

    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority || !Application.isFocused)
        {
            return;
        }

        UpdateCameraPosition();
        UpdateListenerPosition();
        ComputeMinimapIndicator();
    }

    private void PrepareMapIndicator()
    {
        var g = new GameObject("MinimapIndicator");
        m_MinimapIndicator = g.transform;
        g.layer = 11; // put on "Minimap" layer
        m_MinimapIndicator.position = Vector3.zero;
        m_MinimapIndicatorMesh = CreateMinimapIndicatorMesh();
        var mf = g.AddComponent<MeshFilter>();
        mf.mesh = m_MinimapIndicatorMesh;
        var mr = g.AddComponent<MeshRenderer>();
        mr.material = new Material(m_MinimapIndicatorMaterial);
        SetupIndicatorSize();
        ComputeMinimapIndicator();
    }

    private void SetupIndicatorSize()
    {
        var middle = Utils.MiddleOfScreenPointToWorld(groundTargetLayerMask);
        var viewCorners = Utils.ScreenCornersToWorldPoints();
        var w = viewCorners[1].x - viewCorners[0].x;
        var h = viewCorners[2].z - viewCorners[0].z;

        for (var i = 0; i < 4; i++)
        {
            viewCorners[i].x -= middle.x;
            viewCorners[i].z -= middle.z;
        }

        var innerCorners = new Vector3[]
        
        {
                new Vector3(viewCorners[0].x + m_MinimapIndicatorStrokeWidth * w, 0f, viewCorners[0].z + m_MinimapIndicatorStrokeWidth * h),
                new Vector3(viewCorners[1].x - m_MinimapIndicatorStrokeWidth * w, 0f, viewCorners[1].z + m_MinimapIndicatorStrokeWidth * h),
                new Vector3(viewCorners[2].x + m_MinimapIndicatorStrokeWidth * w, 0f, viewCorners[2].z - m_MinimapIndicatorStrokeWidth * h),
                new Vector3(viewCorners[3].x - m_MinimapIndicatorStrokeWidth * w, 0f, viewCorners[3].z - m_MinimapIndicatorStrokeWidth * h)
        };

        var allCorners = new Vector3[]
        {
                viewCorners[0], viewCorners[1], viewCorners[2], viewCorners[3],
                innerCorners[0], innerCorners[1], innerCorners[2], innerCorners[3]
        };

        for (var i = 0; i < 8; i++)
            allCorners[i].y = 100f;
        m_MinimapIndicatorMesh.vertices = allCorners;
        m_MinimapIndicatorMesh.RecalculateNormals();
        m_MinimapIndicatorMesh.RecalculateBounds();
    }

    private Mesh CreateMinimapIndicatorMesh()
    {
        Mesh m = new Mesh();
        Vector3[] vertices = new Vector3[] {
            Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero,
            Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero
        };
        int[] triangles = new int[] {
            0, 4, 1, 4, 5, 1,
            0, 2, 6, 6, 4, 0,
            6, 2, 7, 2, 3, 7,
            5, 7, 3, 3, 1, 5
        };
        m.vertices = vertices;
        m.triangles = triangles;
        return m;
    }

    private void UpdateCameraPosition()
    {
        var pos = playerCameraTransform.position;

        if (previousInput == Vector2.zero)
        {
            var cursorMovement = Vector3.zero;

            var cursorPosition = Mouse.current.position.ReadValue();

            if (cursorPosition.y >= Screen.height - screenBorderThickness)
            {
                cursorMovement.z += 1;
            }
            else if (cursorPosition.y <= screenBorderThickness)
            {
                cursorMovement.z -= 1;
            }

            if (cursorPosition.x >= Screen.width - screenBorderThickness)  
            {
                cursorMovement.x += 1;
            }
            else if (cursorPosition.x <= screenBorderThickness)
            {
                cursorMovement.x -= 1;
            }

            pos += cursorMovement.normalized * speed * Time.deltaTime;
        }
        else
        {
            pos += new Vector3(previousInput.x, 0f, previousInput.y) * speed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, screenXLimit.x, screenXLimit.y);
        pos.z = Mathf.Clamp(pos.z, screenZLimit.x, screenZLimit.y);
        pos.y = Mathf.Lerp(pos.y, UpdateHeight() + m_CameraHeight, 0.1f);

        playerCameraTransform.position = pos;
    }

    private void SetPreviousInput(InputAction.CallbackContext ctx)
    {
        previousInput = ctx.ReadValue<Vector2>(); 
    }

    private void UpdateListenerPosition()
    {
        if (groundTarget == null)
        {
            return;
        }

        var middle = Utils.MiddleOfScreenPointToWorld(groundTargetLayerMask);
        groundTarget.position = middle;
    }

    private float UpdateHeight()
    {
        var middle = Utils.MiddleOfScreenPointToWorld(groundTargetLayerMask);

        return middle.y;
    }

    private void ComputeMinimapIndicator()
    {
        var middle = Utils.MiddleOfScreenPointToWorld(groundTargetLayerMask);

        if (m_MinimapIndicator == null)
        {
            PrepareMapIndicator();
        }

        m_MinimapIndicator.position = middle;
    }

}
