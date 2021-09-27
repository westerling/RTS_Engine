using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : NetworkBehaviour
{
    private Stats m_Stats;
    private Targeter m_Targeter;

    public Stats Stats 
    {
        get => m_Stats;
        set => m_Stats = value; 
    }
    
    public Targeter Targeter 
    {
        get => m_Targeter; 
        set => m_Targeter = value; 
    }

    private void Start()
    {
        var localStats = GetComponent<LocalStats>();
        localStats.StatsAltered += HandleAlteredStats;
        Stats = localStats.Stats;
        Targeter = GetComponent<Targeter>();

    }

    private void HandleAlteredStats(Stats stats)
    {
        Stats = stats;
    }

    [Server]
    protected bool IsCloseEnoughToTarget()
    {
        if (Stats == null)
        {
            return false;
        }

        return (Targeter.Target.transform.position - transform.position).sqrMagnitude <=
            (Stats.GetAttributeAmount(AttributeType.Range) * Stats.GetAttributeAmount(AttributeType.Range));
    }
}
