using System;
using UnityEngine;

public class EnterBuildingAction : ActionBehaviour
{
    [SerializeField]
    private Sprite m_Sprite;

    [SerializeField]
    private SelectionHandler m_SelectionHandler = null;

    private UnitMovement m_UnitMovement = null;
    private bool m_IsSelected = false;

    public bool IsSelected
    {
        get => m_IsSelected;
        set => m_IsSelected = value;
    }

    public override Action GetClickAction()
    {
        return delegate ()
        {
            SetContext(GameContext.Camera);
            m_IsSelected = true;   
        };
    }
}