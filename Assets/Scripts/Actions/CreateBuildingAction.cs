using System;

[Serializable]
public class CreateBuildingAction : ActionBehaviour
{

    private Building m_Building = null;
    private RtsPlayer m_Player;

    public CreateBuildingAction(RtsPlayer player, Building building, int position)
    {
        m_Player = player;
        m_Building = building;

        Position = position;
        PriorUpgrades = building.RequiredUpgrades;
        Icon = building.Icon;
        Id = building.Id;
        Name = building.Name;
        Description = building.Description;
    }


    public override Action GetClickAction()
    {
        return delegate () {
            var stats = m_Building.GetComponent<LocalStats>().Stats;

            if (!Utils.CanAfford(m_Player.GetResources(), stats.GetCost()))
            {
                return;
            }
            SetContext(GameContext.Build);
            Instantiate(m_Building.BuildingPreview);
        };
    }
}
