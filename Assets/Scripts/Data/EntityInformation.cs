using System.Xml.Serialization;

[XmlRoot(ElementName = "Information")]
public class EntityInformation
{
    [XmlElement(ElementName = "Id")]
    public int Id { get; set; }

    [XmlElement(ElementName = "Name")]
    public string EntityName { get; set; }

    [XmlElement(ElementName = "Description")]
    public string Description { get; set; }

    [XmlElement(ElementName = "EntityType")]
    public EntityType EntityType { get; set; }

    [XmlElement(ElementName = "Domain")]
    public Domain Domain { get; set; }

    [XmlElement(ElementName = "MaxNumber")]
    public int MaxNumber { get; set; }

    [XmlElement(ElementName = "RequiredUpgrades")]
    public RequiredUpgradesData RequiredUpgrades { get; set; }
}
