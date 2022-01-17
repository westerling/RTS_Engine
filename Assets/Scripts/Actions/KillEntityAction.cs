using System;
using UnityEngine;

public class KillEntityAction : ActionBehaviour
{
    private Health m_Health;

    public Health Health 
    {
        get => m_Health; 
        set => m_Health = value; 
    }

    public KillEntityAction(int position)
    {
        Position = position;
        Icon = Resources.Load<Sprite>("Sprites/Elements_Death");
        Name = "Kill";
        Description = "Kill this unit";
    }

    public override Action GetClickAction()
    {
        return delegate () 
        {
            Health.CmdSetHealth(0);
        };
    }
}
