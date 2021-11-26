using Mirror;
using UnityEngine;

public class Targeter : BaseUnitClickAction
{
    private Targetable m_Target;

    public Targetable Target
    {
        get { return m_Target; }
        set { m_Target = value; }
    }

    #region server

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        SetTarget(targetGameObject);
    }

    [Server]  
    public void SetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent(out Targetable target))
        {
            return;
        }

        Target = target;
    }

    [Command]
    public void CmdClearTarget()
    {
        ClearTarget();
    }

    [Server]
    public override void ClearTarget()
    {
        Target = null;
    }

    public override void UpdateStats()
    {
        return;
    }

    #endregion

    [ClientRpc]
    public void ClientDebug(string message)
    {
        Debug.Log(message);
    }
}
