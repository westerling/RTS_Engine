using Mirror;
using UnityEngine;

public class TownCenter : Building
{
    [Server]
    public void Deliver(Resource resource, int amount)
    {
        Player.SetResources((int)resource, amount);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Health.CmdSetHealth((int)LocalStats.Stats.GetAttributeAmount(AttributeType.HitPoints));
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Health.SetHealth((int)LocalStats.Stats.GetAttributeAmount(AttributeType.HitPoints));
    }
}
