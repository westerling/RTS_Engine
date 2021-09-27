using System;
using UnityEngine;

[Serializable]
public class UpgradeAttribute : Attribute
{

    [SerializeField]
    private UpgradeEffect m_UpgradeEffect;

    public UpgradeEffect UpgradeEffect
    {
        get { return m_UpgradeEffect; }
    }
}
