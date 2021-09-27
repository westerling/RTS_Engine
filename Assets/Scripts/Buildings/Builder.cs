using Mirror;
using UnityEngine;

public class Builder : BaseUnitClickAction
{
    [SyncVar]
    private Building m_Target;

    public Building Target
    {
        get { return m_Target; }
        set { m_Target = value; }
    }

    #region server

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent(out Building target))
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
}
