using Mirror;
using UnityEngine;

public abstract class TargeterTest : NetworkBehaviour
{
    private Interactable m_Target;

    public Interactable Target
    {
        get { return m_Target; }
        set { m_Target = value; }
    }

    public abstract void UpdateStats();

    public abstract void FindNewTarget(Task task);

    public abstract bool PossibleTask(Task task);

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
    public void ClearTarget()
    {
        Target = null;
    }
}
