using System;
using System.Linq;
using UnityEngine;

public class LocalStats : MonoBehaviour
{
    public event Action<Stats> StatsAltered;

    [SerializeField]
    private Stats m_Stats = null;

    public Stats Stats
    {
        get { return m_Stats; }
        set { m_Stats = value; }
    }

    public void AlterStats(UpgradeStats statsElteration)
    {
        var affectedAttributes = statsElteration.Attributes;

        foreach (var affectedAttribute in affectedAttributes)
        {
            var attributeToUpgrade = Stats.Attributes.FirstOrDefault(x => x.AttributeType == affectedAttribute.AttributeType);

            var newAmount = 0f;

            switch (affectedAttribute.UpgradeEffect)
            {
                case UpgradeEffect.None:
                    break;
                case UpgradeEffect.Addition:
                    newAmount = attributeToUpgrade.Amount + affectedAttribute.Amount;
                    break;
                case UpgradeEffect.Multiplication:
                    newAmount = attributeToUpgrade.Amount * affectedAttribute.Amount;
                    break;
                case UpgradeEffect.NewValue:
                    newAmount = affectedAttribute.Amount;
                    break;
            }
            attributeToUpgrade.Amount = Utils.Round(newAmount, 2);
        }
        StatsAltered?.Invoke(Stats);
    }
}
