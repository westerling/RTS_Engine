using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMob : Mob, IResource, IFoodResource
{
    [SerializeField]
    private FoodType m_FoodType;

    [SerializeField]
    private bool m_IsInfinite = false;

    [SyncVar(hook = nameof(HandleQuantityUpdated))]
    [SerializeField]
    private int m_Quantity = 100;

    [SerializeField]
    private List<GameObject> m_DisableOnDeath = new List<GameObject>();

    private Resource m_Resource = Resource.Food;

    
    public int Quantity
    {
        get => m_Quantity;
        set => m_Quantity = value;
    }

    private void HandleQuantityUpdated(int oldValue, int newValue)
    {
        if (newValue <= 0)
        {
            DestroyThisOnServer();
        }
    }

    public Resource Resource
    {
        get => m_Resource;
    }

    public FoodType FoodType
    {
        get => m_FoodType;
    }

    public bool IsInfinite
    {
        get => m_IsInfinite;
    }

    public bool CanGather
    {
        get
        {
            return IsInfinite || Quantity > 0;
        }
    }

    [Server]
    public bool GatherResources(int gatherAmount)
    {
        if (gatherAmount > Quantity)
        {
            gatherAmount = Quantity;
        }

        Quantity -= gatherAmount;

        if (Quantity <= 0)
        {
            DestroyThisOnServer();
            return false;
        }
        return true;
    }

    public override void ServerHandleDie()
    {
        UnitMovement.enabled = false;
    }
}
