using UnityEngine;

public class Formation : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_Transforms;

    public Transform[] Transforms
    {
        get { return m_Transforms; }
        set { m_Transforms = value; }
    }
}
