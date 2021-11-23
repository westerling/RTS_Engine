using System;
using UnityEngine;

public class OpenWindowAction : ActionBehaviour
{
    [SerializeField]
    private GameObject WindowToOpen = null;

    public override Action GetClickAction()
    {
        return delegate ()
        {
            WindowToOpen.SetActive(true);
        };
    }
}