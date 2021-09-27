using System;
using System.Xml.Serialization;

[Serializable]
public class RequiredBuildingData
{
    [XmlElement(ElementName = "RequiredBuildingId")]
    public int RequiredBuildingId { get; set; }
}
