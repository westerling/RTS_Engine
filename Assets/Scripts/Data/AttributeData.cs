using System;
using System.Xml.Serialization;

[Serializable]
public class AttributeData
{
    [XmlElement(ElementName = "Amount")]
    public float Amount { get; set; }

    [XmlElement(ElementName = "AttributeType")]
    public AttributeType AttributeType { get; set; }
}