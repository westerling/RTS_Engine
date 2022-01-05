using System;
using UnityEngine;

public class SwitchPanelsAction : ActionBehaviour
{
    public SwitchPanelsAction(Sprite sprite, int position)
    {
        Position = position;
        Icon = sprite;
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
