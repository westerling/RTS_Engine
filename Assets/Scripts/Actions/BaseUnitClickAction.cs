using Mirror;
using System.Collections.Generic;

public abstract class BaseUnitClickAction : NetworkBehaviour
{
    //public abstract void UpdateStats();

    //public abstract void ClearTarget();

    //public override void OnStartServer()
    //{
    //    UpdateStats();

    //    GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    //    GetComponent<LocalStats>().StatsAltered += HandleStatsAltered;
    //}

    //public override void OnStopServer()
    //{
    //    GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    //    GetComponent<LocalStats>().StatsAltered -= HandleStatsAltered;
    //}

    //[Server]
    //private void ServerHandleGameOver()
    //{
    //    ClearTarget();
    //}

    

    //public virtual void HandleStatsAltered(Stats stats)
    //{
    //}

    //private void HandleUpgradeAlert(List<int> obj)
    //{
    //    var id = GetComponent<GameObjectIdentity>().Id;

    //    if (!obj.Contains(id))
    //    {
    //        return;
    //    }

    //    UpdateStats();
    //}
}
