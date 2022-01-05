using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    [SerializeField]
    private bool m_Attacker = false;

    [SerializeField]
    private bool m_Collector = false;

    [SerializeField]
    private bool m_Builder = false;

    [SerializeField]
    private Interactable m_Target;

    [SerializeField]
    private Interactable m_DropOff;
    
    public Interactable Target
    {
        get => m_Target;
        set => m_Target = value;
    }

    public Interactable DropOff 
    {
        get => m_DropOff; 
        set => m_DropOff = value;
    }

    public bool Attacker 
    {
        get => m_Attacker; 
    }

    public bool Collector 
    { 
        get => m_Collector; 
    }

    public bool Builder 
    { 
        get => m_Builder;
    }

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    public bool FindNewTarget(Task task)
    {
        var player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        switch (task)
        {
            case Task.Build:
                Target = TargetFinder.FindNewBuilding(player, transform);
                break;
            case Task.Attack:
                Target = TargetFinder.FindNewEnemyUnit(transform, 10f);
                break;
        }

        return Target == null;
    }

    public bool FindNewTarget(Task task, float range)
    {
        switch (task)
        {
            case Task.Attack:
                Target = TargetFinder.FindNewEnemyUnit(transform, range);
                break;
        }

        return Target == null;
    }

    public bool FindNewTarget(Task task, Resource resource)
    {
        var player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        switch (task)
        {
            case Task.Collect:
                Target = TargetFinder.FindNewResource(transform, resource);
                break;
            case Task.Deliver:
                Target = TargetFinder.FindClosestDropoff(player, transform, resource);
                break;
        }

        return Target == null;
    }

    public bool PossibleTask(Task task)
    {
        switch (task)
        {
            case Task.Idle:
            case Task.Move:
            case Task.Garrison:
                return true;
            case Task.Build:
                return Builder;
            case Task.Attack:
                return Attacker;
            case Task.Deliver:
            case Task.Collect:
                return Collector;
        }
        return false;
    }

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        SetTarget(targetGameObject);
    }

    [Server]
    public void SetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent(out Interactable target))
        {
            return;
        }

        Target = target;
    }

    [Command]
    public void CmdSetDropOff(GameObject targetGameObject)
    {
        SetDropOff(targetGameObject);
    }

    [Server]
    public void SetDropOff(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent(out IDropOff dropOff))
        {
            return;
        }

        DropOff = targetGameObject.GetComponent<Building>();
    }

    [Command]
    public void CmdClearTarget()
    {
        ClearTarget();
    }

    [Server]
    public void ClearTarget()
    {
        DropOff = null;
        Target = null;
    }

    [Server]
    private void ServerHandleGameOver()
    {
        ClearTarget();
    }

    [ClientRpc]
    public void ClientDebug(string message)
    {
        Debug.Log(message);
    }
}
