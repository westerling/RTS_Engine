using Mirror;
using UnityEngine;

public class TargeterOld : BaseUnitClickAction
{
    private InteractableGameEntity m_Target;

    public InteractableGameEntity Target
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
        if (!targetGameObject.TryGetComponent(out InteractableGameEntity target))
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
    public void ClearTarget()
    {
        Target = null;
    }

    public void UpdateStats()
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
