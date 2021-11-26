using System;
using UnityEngine;

public class ActionButton
{
    private Sprite m_Sprite;
    private Action m_ClickAction;
    private Action m_HoverAction;
    private int m_Position;
    private string m_Description;
    
    public ActionButton(Sprite sprite, string description, Action clickAction, int position)
    {
        Sprite = sprite;
        Description = description;
        ClickAction = clickAction;
        Position = position;
    }

    public ActionButton()
    {
        ClickAction = BaseClickAction();
        Position = -1;
    }

    public Sprite Sprite 
    { 
        get => m_Sprite;
        set => m_Sprite = value;
    }

    public Action ClickAction
    {
        get => m_ClickAction; 
        set => m_ClickAction = value; 
    }

    public string Description
    {
        get => m_Description;
        set => m_Description = value;
    }

    public int Position
    {
        get => m_Position; 
        set => m_Position = value; 
    }

    public Action BaseClickAction()
    {
        return delegate ()
        {
        };
    }

}
