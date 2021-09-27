using System;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : GameObjectIdentity
{
    public static event Action<Upgrade> ServerOnUpgradeAdded;
    public static event Action<Upgrade> AuthorityOnUpgradeAdded;

    [SerializeField]
    private List<Affected> m_AffectedEntities = new List<Affected>();
    
    [SerializeField]
    private bool m_Ping = false;

    [SerializeField]
    private List<int> m_RequiredBuildings = new List<int>();

    public bool Ping
    {
        get { return m_Ping; }
    }

    public List<Affected> AffectedEntities
    {
        get { return m_AffectedEntities; }
    }

    public List<int> RequiredBuildings
    {
        get { return m_RequiredBuildings; }
        set { m_RequiredBuildings = value; }
    }

    public override void OnStartServer()
    {
        ServerOnUpgradeAdded?.Invoke(this);
    }

    public override void OnStartAuthority()
    {
        AuthorityOnUpgradeAdded?.Invoke(this);
    }
}
