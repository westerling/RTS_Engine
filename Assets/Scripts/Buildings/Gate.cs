using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gate : Building
{
    [SerializeField]
    private NavMeshObstacle m_Portcullis;

    private List<Unit> m_Units = new List<Unit>();

    public NavMeshObstacle Portcullis
    {
        get => m_Portcullis; 
        set => m_Portcullis = value; 
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        m_Portcullis.enabled = false;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            if (unit.hasAuthority)
            {
                m_Units.Add(unit);
            }
        }

        UpdatePortcullis();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            if (unit.hasAuthority)
            {
                if (m_Units.Contains(unit))
                {
                    m_Units.Remove(unit);
                }
            }
        }

        UpdatePortcullis();
    }

    private void UpdatePortcullis()
    {
        m_Portcullis.enabled  = m_Units.Count > 0;
    }
}
