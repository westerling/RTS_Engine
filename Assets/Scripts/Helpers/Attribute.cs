using System;
using UnityEngine;

[Serializable]
public class Attribute
{
    [SerializeField]
    private float m_Amount;
    
    [SerializeField]
    private AttributeType m_AttributeType;

    public AttributeType AttributeType
    {
        get { return m_AttributeType; }
    }

    public float Amount
    {
        get { return m_Amount; }
        set { m_Amount = value; }
    }
}
