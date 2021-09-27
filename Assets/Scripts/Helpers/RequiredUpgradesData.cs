using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class RequiredUpgradesData
{
    [XmlElement(ElementName = "Upgrade")]
    public List<RequiredUpgradeData> RequiredUpgrade { get; set; }
}
