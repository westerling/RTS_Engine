using System;
using UnityEngine;

public class SwitchPanelsAction : ActionBehaviour
{
    [SerializeField]
    private Sprite m_Sprite;

    private void Start()
    {
        Icon = m_Sprite;
        Name = "Switch";
        Description = "Switch Page";
    }

    public override Action GetClickAction()
    {
        return delegate ()
        {
            ActionsDisplay.Current.TogglePanels();
        };
    }
}
