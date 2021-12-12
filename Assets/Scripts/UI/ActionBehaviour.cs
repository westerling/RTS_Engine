using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class ActionBehaviour : MonoBehaviour
{
    public abstract Action GetClickAction();

    [SerializeField]
    private int m_Position;

    private List<int> m_PriorUpgrades = new List<int>();
    private Sprite m_Icon;
    private int m_Id = -1;
    private string m_EntityName;
    private string m_Description;

    public Sprite Icon
    {
        get => m_Icon;
        set => m_Icon = value;
    }

    public int Id
    {
        get => m_Id;
        set => m_Id = value;
    }

    public string Name
    {
        get => m_EntityName;
        set => m_EntityName = value;
    }    
    
    public string Description
    {
        get => m_Description;
        set => m_Description = value;
    }

    public List<int> PriorUpgrades
    {
        get => m_PriorUpgrades;
        set => m_PriorUpgrades = value;
    }

    public int Position 
    {
        get => m_Position; 
        set => m_Position = value; 
    }

    protected void SetContext(GameContext context)
    {
        InputManager.Current.SetContext(context);
    }
}
