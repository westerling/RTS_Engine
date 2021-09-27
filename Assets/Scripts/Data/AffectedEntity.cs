using System;
using System.Xml.Serialization;

[Serializable]
public class AffectedEntity
{
    [XmlElement(ElementName = "AffectedEntityId")]
    public int AffectedEntityId { get; set; }
}
