using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : NetworkBehaviour
{
    [SerializeField]
    private Stats m_Stats;

    private int m_Id;

    public void SetStats(Stats newStats)
    {
        m_Stats = newStats;
    }

    public Stats Stats
    {
        get { return m_Stats; }
        set { m_Stats = value; }
    }

    public int Id
    {
        get { return m_Id; }
        set { m_Id = value; }
    }

    public override void OnStartServer()
    {

        SetEntityId();
        UpdateLocalStats();
    }

    public override void OnStopServer()
    {
    }

    private void HandleUpgradeAlert(List<int> obj)
    {
        if (!obj.Contains(m_Id))
        {
            return;
        }

        UpdateLocalStats();
    }

    private void SetEntityId()
    {
        Id = GetComponent<GameObjectIdentity>().Id;
    }

    public virtual void UpdateLocalStats()
    {
    }
}
