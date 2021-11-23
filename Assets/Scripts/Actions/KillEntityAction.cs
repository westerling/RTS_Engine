using System;
using UnityEngine;

public class KillEntityAction : ActionBehaviour
{
    [SerializeField]
    private Sprite m_Sprite;

    private void Start()
    {
        Icon = m_Sprite;
        Name = "Kill";
        Description = "Kill this unit";
    }

    public override Action GetClickAction()
    {
        return delegate () 
        {
            GetComponent<Health>().CmdSetHealth(0);
        };
    }
}
