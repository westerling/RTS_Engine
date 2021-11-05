using Mirror;
using System;

public class StartupBuilding : NetworkBehaviour
{
    public static event Action AuthorityOnStartupBuildingAdded;
    public static event Action ServerOnStartupBuildingAdded;

    public override void OnStartAuthority()
    {
        AuthorityOnStartupBuildingAdded?.Invoke();
    }

    public override void OnStartServer()
    {   
            ServerOnStartupBuildingAdded?.Invoke();   
    }
}
