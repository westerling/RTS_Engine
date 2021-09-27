using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class RequiredBuildingsData
{
    [XmlElement(ElementName = "RequiredBuilding")]
    public List<RequiredBuildingData> RequiredBuilding { get; set; }
}
