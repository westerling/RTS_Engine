using Mirror;
using UnityEngine;
using System;

public class CreateUnitAction : ActionBehaviour
{
    [SerializeField]
    private Unit m_Unit = null;
    
    private Spawner m_Spawner = null;
    private RtsPlayer m_Player;

    private void Start()
    {
        m_Player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
        m_Spawner = GetComponent<Spawner>();

        PriorUpgrades = m_Unit.RequiredUpgrades;
        Icon = m_Unit.Icon;
        Id = m_Unit.Id;
        Name = m_Unit.Name;
        Description = m_Unit.Description;
    }

    public override Action GetClickAction()
    {
        return delegate () {
            var stats = m_Unit.GetComponent<LocalStats>().Stats;
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

            m_Spawner.CmdAddToQueue(Id);
        };
    }
}
