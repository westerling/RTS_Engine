using UnityEngine;

public class CollectableFood : Collectable
{
    [SerializeField]
    private FoodType m_FoodType;

    public FoodType FoodType
    {
        get => m_FoodType;
    }
}
