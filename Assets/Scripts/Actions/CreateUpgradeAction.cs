using System;

public class CreateUpgradeAction : ActionBehaviour
{    
    private Upgrade m_Upgrade = null;
    private Spawner m_Spawner = null;
    private RtsPlayer m_Player;

    public CreateUpgradeAction(RtsPlayer player, Upgrade upgrade, Spawner spawner, int position)
    {
        m_Player = player;
        m_Upgrade = upgrade;
        m_Spawner = spawner;

        Position = position;
        PriorUpgrades = upgrade.RequiredUpgrades;
        Icon = upgrade.Icon;
        Id = upgrade.Id;
        Name = upgrade.Name;
        Description = upgrade.Description;
    }

    public override Action GetClickAction()
    {
        return delegate () {
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
