using Mirror;
using UnityEngine;

public class Collectable : Interactable, IResource
{
    [SerializeField]
    private Resource m_Resource;

    [SerializeField]
    private bool m_IsInfinite = false;

    [SerializeField]
    private int m_Quantity = 100;

    private int m_CurrentGatherers;
    private int m_MaxGatherers = 10;

    public Resource Resource
    {
        get => m_Resource;
    }

    public int Quantity
    {
        get => m_Quantity;
        set => m_Quantity = value;
    }

    public bool IsInfinite
    {
        get => m_IsInfinite;
    }

    public int CurrentGatherers
    {
        get => m_CurrentGatherers;
    }

    public int MaxGatherers
    {
        get => m_MaxGatherers;    
    }

    public bool CanGather
    {
        get { return IsInfinite || Quantity > 0; }
    }

    [Server]
    public bool GatherResources(int gatherAmount)
    {
        if (gatherAmount > m_Quantity)
        {
            gatherAmount = m_Quantity;
        }

        m_Quantity -= gatherAmount;

        if (Quantity <= 0)
        {
            Destroy(this.gameObject);
            return false;
        }
        return true;
    }
}
