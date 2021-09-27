using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot(ElementName = "Units")]
public class UnitsData
{
    [XmlElement(ElementName = "Unit")]
    public List<UnitData> UnitData { get; set; }
}

[Serializable]
public class UnitData : BaseGameObjectData
{
}