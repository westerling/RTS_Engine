using Mirror;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameObjectIdentity : NetworkBehaviour
{    
    [SerializeField]
    private Sprite m_Icon = null;
    
    [SerializeField]
    private int m_Id = -1;

    [SerializeField]
    private EntityType m_EntityType = EntityType.None;

    [SerializeField]
    private string m_EntityName = "Unknown";

    [SerializeField]
    private string m_Description = "This is an unknown object.";

    [SerializeField]
    private List<int> m_RequiredUpgrades = new List<int>();

    public Sprite Icon
    {
        get { return m_Icon; }
    }

    public int Id
    {
        get { return m_Id; }
    }

    public string Name 
    { 
        get { return m_EntityName; }
    }

    public string Description
    {
        get { return m_Description; }
    }

    public EntityType EntityType
    {
        get { return m_EntityType; }
    }    
    
    public List<int> RequiredUpgrades
    {
        get { return m_RequiredUpgrades; }
    }

    //public void ReadStats()
    //{
    //    if (m_Information == null)
    //    {
    //        return;
    //    }

    //    var stream = new MemoryStream(m_Information.bytes);
    //    var info = Utils.Deserialize<EntityInformation>(stream);

    //    if (info == null)
    //    {
    //        return;
    //    }

    //    Id = info.Id;
    //    Name = info.EntityName;
    //    Description = info.Description;
    //    Domain = info.Domain;
    //    EntityType = info.EntityType;
    //    MaxNumber = info.MaxNumber;
    //    RequiredUpgrades = CreateRequiredUpgradesList(info.RequiredUpgrades.RequiredUpgrade);
    //}

    //private List<int> CreateRequiredUpgradesList(List<RequiredUpgradeData> requiredUpgrades)
    //{
    //    var requiredUpgradeList = new List<int>();

    //    foreach (var requiredUpgrade in requiredUpgrades)
    //    {
    //        requiredUpgradeList.Add(requiredUpgrade.RequiredUpgradeId);
    //    }

    //    return requiredUpgradeList;
    //}
}
