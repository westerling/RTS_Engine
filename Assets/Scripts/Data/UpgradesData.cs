using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot(ElementName = "Upgrades")]
public class UpgradesData
{
    [XmlElement(ElementName = "Upgrade")]
    public List<UpgradeData> UpgradeData { get; set; }
}

[Serializable]
public class UpgradeData : BaseGameObjectData
{
    [XmlElement(ElementName = "UpgradeEffect")]
    public UpgradeEffect UpgradeEffect { get; set; }

    [XmlElement(ElementName = "RequiredBuildings")]
    public RequiredBuildingsData RequiredBuildings { get; set; }

    [XmlElement(ElementName = "AffectedEnteties")]
    public AffectedEntetiesData AffectedEnteties { get; set; }

    [XmlElement(ElementName = "AffectedAttributes")]
    public AttributesData AffectedAttributes { get; set; }
}




