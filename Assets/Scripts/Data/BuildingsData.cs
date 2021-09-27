using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot(ElementName = "Buildings")]
public class BuildingsData
{
    [XmlElement(ElementName = "Building")]
    public List<BuildingData> Buildings { get; set; }
}

[Serializable]
public class BuildingData : BaseGameObjectData
{
}