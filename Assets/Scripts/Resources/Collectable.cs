using UnityEngine;

public class Collectable : Interactable
{
    [SerializeField]
    private Resource m_Resource;

    [SerializeField]
    private bool isInfinite = false;

    [SerializeField]
    private int m_Quantity = 100;

    private int m_CurrentGatherers;
    private int m_MaxGatherers = 10;

    public Resource Resource
    {
        get => m_Resource;
    }

    

    public int CurrentGatherers
    {
        get => m_CurrentGatherers;
    }

    public int MaxGatherers
    {
        get => m_MaxGatherers;    
    }

    public int Quantity
    {
        get => m_Quantity;
    }

    public bool CanGather()
    {
        return isInfinite || Quantity > 0;
    }

    public void GatherResources(int gatherAmount)
    {
        if (gatherAmount > m_Quantity)
        {
            gatherAmount = m_Quantity;
        }

        m_Quantity -= gatherAmount;

        if (Quantity <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
