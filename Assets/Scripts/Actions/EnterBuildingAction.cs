using System;
using UnityEngine;

public class EnterBuildingAction : ActionBehaviour
{
    public EnterBuildingAction(Sprite sprite, int position)
    {
        Icon = sprite;
        Position = position;
    }

    public override Action GetClickAction()
    {
        return delegate ()
        {
            InputManager.Current.SetContext(GameContext.Garrison);
        };
    }
}