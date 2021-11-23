using System;
using UnityEngine;

public class ActionButton
{
    private Sprite m_Sprite;
    private Action m_ClickAction;
    private int m_Position;
    
    public ActionButton(Sprite sprite, Action clickAction, int position)
    {
        Sprite = sprite;
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
