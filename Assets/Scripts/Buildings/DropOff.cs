using Mirror;
using UnityEngine;

public class DropOff : Building, IDropOff
{
    [SerializeField]
    private Resource m_Resource;
    
    public Resource Resource
    {
        get { return m_Resource; }
        set { m_Resource = value; }
    }

    [Server]
    public void Deliver(int amount)
    {
        Player.SetResources((int)m_Resource, amount);
    }
}
