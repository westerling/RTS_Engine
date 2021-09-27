using System;
using UnityEngine;

[Serializable]
public class Affected
{
    [SerializeField]
    private GameObject m_AffectedGameObject = null;

    [SerializeField]
    private UpgradeStats m_Stats;

    public GameObject AffectedGameObject
    {
        get { return m_AffectedGameObject; }
    }

    public UpgradeStats Stats
    {
        get { return m_Stats; }
    }
}
