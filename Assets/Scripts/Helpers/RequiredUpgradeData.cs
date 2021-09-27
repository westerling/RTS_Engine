using System;
using System.Xml.Serialization;

[Serializable]
public class RequiredUpgradeData
{
    [XmlElement(ElementName = "RequiredUpgradeId")]
    public int RequiredUpgradeId { get; set; }
}
