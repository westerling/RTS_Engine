using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeStats
{
    [SerializeField]
    private List<UpgradeAttribute> m_Attributes = new List<UpgradeAttribute>();

    public List<UpgradeAttribute> Attributes
    {
        get => m_Attributes; 
        set => m_Attributes = value;
    }
}
