using System;

public class CreateUnitAction : ActionBehaviour
{
    private Unit m_Unit = null;
    private Spawner m_Spawner = null;
    private RtsPlayer m_Player;

    public CreateUnitAction(RtsPlayer player, Unit unit, Spawner spawner, int position)
    {
        m_Player = player;
        m_Unit = unit;
        m_Spawner = spawner;

        Position = position;
        PriorUpgrades = unit.RequiredUpgrades;
        Icon = unit.Icon;
        Id = unit.Id;
        Name = unit.Name;
        Description = unit.Description;
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
