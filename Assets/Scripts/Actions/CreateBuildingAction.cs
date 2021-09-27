using Mirror;
using UnityEngine;
using System;

[Serializable]
public class CreateBuildingAction : ActionBehaviour
{
    [SerializeField]
    private Building m_Building = null;

    private RtsPlayer m_Player;

    private void Start()
    {
        m_Player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        PriorUpgrades = m_Building.RequiredUpgrades;
        Icon = m_Building.Icon;
        Id = m_Building.Id;
        Name = m_Building.Name;
        Description = m_Building.Description;
    }

    public override Action GetClickAction()
    {
        return delegate () {
            //var stats = m_StatsManager.GetBuildingStats(Id);
            var stats = m_Building.GetComponent<LocalStats>().Stats;

            if (!Utils.CanAfford(m_Player.GetResources(), stats.GetCost()))
            {
                return;
            }

            Instantiate(m_Building.GetBuildingPreview());
        };
    }
}
