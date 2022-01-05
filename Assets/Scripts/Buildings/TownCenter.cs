using Mirror;

public class TownCenter : Building, IDropOff
{
    private Resource m_Resource;

    public Resource Resource 
    {
        get => m_Resource;
        set => m_Resource = value;
    }

    [Server]
    public void Deliver(int amount)
    {
        RpcDeliver(amount);
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

    #region Client
    [ClientRpc]
    public void RpcDeliver(int amount)
    {
        Player.CmdSetResources((int)Resource, amount);
    }
    #endregion
}
