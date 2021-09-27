using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Stats
{    
    [SerializeField]
    private List<Attribute> m_Attributes = new List<Attribute>();
    
    public List<Attribute> Attributes
    {
        get { return m_Attributes; }
        set { m_Attributes = value; }
    }

    public float GetAttributeAmount(AttributeType attributeType)
    {
        try
        {
            var attribte = Attributes.Where(attribute => attribute.AttributeType == attributeType).Select(attribute => attribute.Amount).DefaultIfEmpty(0);
            return attribte.FirstOrDefault();
        }
        catch (Exception)
        {
            Debug.LogError("Cannot find attribute of type " + attributeType);
            throw;
        }     
    }

    public IDictionary<Resource, int> GetCost()
    {
        IDictionary<Resource, int> d = new Dictionary<Resource, int>();
        d.Add(new KeyValuePair<Resource, int>(Resource.Food, (int)GetAttributeAmount(AttributeType.CostFood)));
        d.Add(new KeyValuePair<Resource, int>(Resource.Gold, (int)GetAttributeAmount(AttributeType.CostGold)));
        d.Add(new KeyValuePair<Resource, int>(Resource.Stone, (int)GetAttributeAmount(AttributeType.CostStone)));
        d.Add(new KeyValuePair<Resource, int>(Resource.Wood, (int)GetAttributeAmount(AttributeType.CostWood)));

        return d;
    }
}
