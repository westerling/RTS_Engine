using System.Xml.Serialization;

public class BaseGameObjectData
{
    [XmlElement(ElementName = "Id")]
    public int Id { get; set; }

    [XmlElement(ElementName = "Attributes")]
    public AttributesData Attributes { get; set; }
}
