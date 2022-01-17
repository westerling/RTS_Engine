using System;
using UnityEngine;

public class SwitchPanelsAction : ActionBehaviour
{
    public SwitchPanelsAction(int pos)
    {
        Position = pos;
        Icon = Resources.Load<Sprite>("Sprites/Modifiers_LevelUp");
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
