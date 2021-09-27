using Mirror;
using UnityEngine;
using System;

public class CreateUpgradeAction : ActionBehaviour
{    
    [SerializeField]
    private Upgrade m_Upgrade = null;

    private Spawner m_Spawner = null;
    private RtsPlayer m_Player;

    private void Start()
    {
        m_Player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
        m_Spawner = GetComponent<Spawner>();

        PriorUpgrades = m_Upgrade.RequiredUpgrades;
        Icon = m_Upgrade.Icon;
        Id = m_Upgrade.Id;
        Name = m_Upgrade.Name;
        Description = m_Upgrade.Description;
    }

    public override Action GetClickAction()
    {
        return delegate () {
            //var stats = m_StatsManager.GetUpgradeStats(Id);
            var stats = m_Upgrade.GetComponent<LocalStats>().Stats;
            var resources = m_Player.GetResources();

            if (!Utils.CanAfford(m_Player.GetResources(), stats.GetCost()) ||
                m_Spawner == null)
            {
                return;
            }

            foreach (var resourceCostItem in stats.GetCost())
            {
                m_Player.CmdSetResources((int)resourceCostItem.Key, -resourceCostItem.Value);
            }

            m_Spawner.CmdAddToQueue(m_Upgrade.Id);
        };
    }
}
