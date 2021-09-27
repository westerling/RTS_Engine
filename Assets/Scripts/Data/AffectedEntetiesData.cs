using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class AffectedEntetiesData
{
    [XmlElement(ElementName = "AffectedEntity")]
    public List<AffectedEntity> AffectedEntity { get; set; }
}
