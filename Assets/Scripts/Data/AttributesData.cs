using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class AttributesData
{

    [XmlElement(ElementName = "Attribute")]
    public List<AttributeData> Attribute { get; set; }
}