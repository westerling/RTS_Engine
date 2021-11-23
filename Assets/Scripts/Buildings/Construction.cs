using Mirror;
using System;
using UnityEngine;

public class Construction : Targetable
{
    //[SerializeField]
    //private Building m_Building = null;

    //[SerializeField]
    //private Health m_Health = null;

    //public static event Action<Construction> ServerOnConstructionStarted;

    //public static event Action<Construction> AuthorityOnConstructionStarted;

    //public override void OnStartAuthority()
    //{
    //    AuthorityOnConstructionStarted?.Invoke(this);
    //}

    //public override void OnStartServer()
    //{


    //    ServerOnConstructionStarted?.Invoke(this);

    //    m_Health.EventHealthChanged += RpcHandleHealthChanged;

    //    Player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
    //}

    public override void Reaction(GameObject sender)
    {
        return;
    }
}
