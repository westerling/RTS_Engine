using Mirror;

public class TownCenter : Building
{
    [Server]
    public void Deliver(Resource resource, int amount)
    {
        Player.SetResources((int)resource, amount);
    }
}
