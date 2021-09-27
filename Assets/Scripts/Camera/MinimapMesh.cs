
using UnityEngine;

public class MinimapMesh : MonoBehaviour
{
    public Material m_MinimapIndicatorMaterial;
    private float m_MinimapIndicatorStrokeWidth = 0.1f; // relative to indicator size
    private Transform m_MinimapIndicator;
    private Mesh m_MinimapIndicatorMesh;

    private void Awake()
    {
        PrepareMapIndicator();
    }

    private void TranslateCamera(int dir)
    {
        ComputeMinimapIndicator(false);
    }

    private void Zoom(int zoomDir)
    {
        ComputeMinimapIndicator(true);
    }

    private void PrepareMapIndicator()
    {
        GameObject g = new GameObject("MinimapIndicator");
        m_MinimapIndicator = g.transform;
        g.layer = 11; // put on "Minimap" layer
        m_MinimapIndicator.position = Vector3.zero;
        m_MinimapIndicatorMesh = CreateMinimapIndicatorMesh();
        MeshFilter mf = g.AddComponent<MeshFilter>();
        mf.mesh = m_MinimapIndicatorMesh;
        MeshRenderer mr = g.AddComponent<MeshRenderer>();
        mr.material = new Material(m_MinimapIndicatorMaterial);
        ComputeMinimapIndicator(true);
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

    private void ComputeMinimapIndicator(bool zooming)
    {
        Vector3 middle = Utils.MiddleOfScreenPointToWorld();
        // if zooming: recompute the indicator mesh
        if (zooming)
        {
            Vector3[] viewCorners = Utils.ScreenCornersToWorldPoints();
            float w = viewCorners[1].x - viewCorners[0].x;
            float h = viewCorners[2].z - viewCorners[0].z;
            for (int i = 0; i < 4; i++)
            {
                viewCorners[i].x -= middle.x;
                viewCorners[i].z -= middle.z;
            }
            Vector3[] innerCorners = new Vector3[]
            {
                new Vector3(viewCorners[0].x + m_MinimapIndicatorStrokeWidth * w, 0f, viewCorners[0].z + m_MinimapIndicatorStrokeWidth * h),
                new Vector3(viewCorners[1].x - m_MinimapIndicatorStrokeWidth * w, 0f, viewCorners[1].z + m_MinimapIndicatorStrokeWidth * h),
                new Vector3(viewCorners[2].x + m_MinimapIndicatorStrokeWidth * w, 0f, viewCorners[2].z - m_MinimapIndicatorStrokeWidth * h),
                new Vector3(viewCorners[3].x - m_MinimapIndicatorStrokeWidth * w, 0f, viewCorners[3].z - m_MinimapIndicatorStrokeWidth * h)
            };
            Vector3[] allCorners = new Vector3[]
            {
                viewCorners[0], viewCorners[1], viewCorners[2], viewCorners[3],
                innerCorners[0], innerCorners[1], innerCorners[2], innerCorners[3]
            };
            for (int i = 0; i < 8; i++)
                allCorners[i].y = 100f;
            m_MinimapIndicatorMesh.vertices = allCorners;
            m_MinimapIndicatorMesh.RecalculateNormals();
            m_MinimapIndicatorMesh.RecalculateBounds();
        }
        // move the game object at the center of the main camera screen
        m_MinimapIndicator.position = middle;
    }
}
