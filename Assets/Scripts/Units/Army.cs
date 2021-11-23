using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Army : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_Positions = new List<GameObject>();
    
    private List<Unit> m_Units = new List<Unit>();

    private NavMeshAgent m_Agent = null;

    private void Start()
    {

    }
}
