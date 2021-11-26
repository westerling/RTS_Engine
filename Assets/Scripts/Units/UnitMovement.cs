using UnityEngine;
using UnityEngine.AI;
using Mirror;
using System.Collections.Generic;
using System.Collections;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField]
    private NavMeshAgent m_Agent = null;

    [SerializeField]
    private Unit m_Unit = null;

    [SerializeField]
    private Targeter m_Targeter = null;

    [SerializeField]
    private Collector m_Collector = null;

    [SerializeField]
    private Builder m_Builder = null;

    [SerializeField]
    private Collider m_Collider = null;

    [SyncVar]
    private List<Vector3> m_MovementList = new List<Vector3>();

    private float m_Timer = 1;

    [SerializeField]
    [SyncVar]
    private Task m_Task;

    public Task Task
    {
        get => m_Task;
        set => m_Task = value;
    }

    public List<Vector3> MovementList
    {
        get => m_MovementList;
        set => m_MovementList = value;
    }

    #region server

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }


    [ServerCallback]
    private void Update()
    {
        if (MovementList.Count <= 0)
        {
            m_Agent.ResetPath();
            return;
        }

        if (Task == Task.Attack)
        {
            StartCoroutine(UpdateTargetPosition());
            m_Timer -= Time.deltaTime;
        }

        m_Agent.SetDestination(MovementList[0]);

        if (m_Agent.remainingDistance <= m_Agent.stoppingDistance && !m_Agent.pathPending)
        {
            MovementList.RemoveAt(0);
            return;
        }
    }

    [Command]
    public void CmdCollect()
    {
        Collect();
    }

    [Command]
    public void CmdDeliver()
    {
        Deliver();
    }

    [Command]
    public void CmdBuild()
    {
        Build();
    }

    [Command]
    public void CmdAttack()
    {
        Attack();
    }

    [Command]
    public void CmdGarrison()
    {
        Garrison();
    }

    [Command]
    public void CmdAddPoint(Vector3 position)
    {
        ServerAddPoint(position);
    }

    [Command]
    public void CmdMove(Vector3 position)
    {  
        ServerMove(position);
    }

    [Command]
    public void CmdMovePriority(Vector3 position)
    {
        ServerMovePriority(position);
    }

    [Command]
    public void CmdSetTask(int newTask)
    {
        SetTask(newTask);
    }

    [Server]
    public void SetTask(int newTask)
    {
        Task = (Task)newTask;
    }

    [Server]
    public void ServerMove(Vector3 position, bool autoMove = false, bool resetTargets = true)
    {
        if (resetTargets)
        {
            m_Targeter.ClearTarget();
        }

        ClearMovementList();

        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas) && !autoMove)
        {
            return;
        }

        MovementList.Add(position);
    }

    [Server]
    public void ServerMovePriority(Vector3 position, bool autoMove = false)
    {
        m_Targeter.ClearTarget();

        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas) && !autoMove)
        {
            return;
        }

        MovementList.Insert(0, position);
    }

    [Server]
    public void ServerAddPoint(Vector3 position, bool autoMove = false)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas) && !autoMove)
        {
            return;
        }

        MovementList.Add(position);
    }

    [Server]
    public void ClearMovementList()
    {
        MovementList.Clear();
    }

    [Server]
    private void ServerHandleGameOver()
    {
        m_Agent.ResetPath();
    }

    [Server]
    public void Deliver()
    {
        var target = m_Collector.DeliveryPoint;
        if (target == null)
        {
            Task = Task.Idle;
            return;
        }

        if ((target.transform.position - transform.position).sqrMagnitude > 2f * 2f)
        {
            m_Agent.stoppingDistance = Utils.GameObjectSize(target.GetComponent<Building>().Size);
            Task = Task.Deliver;
            ServerMove(target.transform.position, true);
        }
    }

    [Server]
    public void Collect()
    {
        var target = m_Collector.Target;

        if (target == null)
        {
            Task = Task.Idle;
            return;
        }

        if ((target.transform.position - transform.position).sqrMagnitude > 2f * 2f)
        {
            m_Agent.stoppingDistance = Utils.GameObjectSize(target.GetComponent<Interactable>().Size);
            Task = Task.Collect;
            ServerMove(Utils.OffsetPoint(target.transform.position, 1), true);
        }
    }

    [Server]
    public void Build()
    {
        var target = m_Builder.Target;

        if (target == null)
        {
            Task = Task.Idle;
            return;
        }

        if (target.GetComponent<Health>().HasFullHealth())
        {
            m_Builder.CmdClearTarget();
            Task = Task.Idle;
        }

        //if ((target.transform.position - transform.position).sqrMagnitude > 2f * 2f)
        //{
            m_Agent.stoppingDistance = Utils.GameObjectSize(target.GetComponent<Building>().Size);
            Task = Task.Build;
            ServerMove(target.transform.position, true);
        //}
    }

    [Server]
    public void Attack(bool resetTargets = false)
    {
        var target = m_Targeter.Target;
        if (target == null)
        {
            Task = Task.Idle;
            return;
        }

        var stats = GetComponent<LocalStats>().Stats;
        var range = stats.GetAttributeAmount(AttributeType.Range);
        var targetSize = Utils.GameObjectSize(target.Size);

        Task = Task.Attack;

        var stoppDist = range + targetSize;
        ClientDebug("dist: " + stoppDist);
        m_Agent.stoppingDistance = range + targetSize;
        ServerMove(target.transform.position, true, resetTargets);
    }

    [Server]
    public void Garrison()
    {
        var target = m_Unit.Targeter.Target;

        if (target == null)
        {
            return;
        }

        var targetSize = Utils.GameObjectSize(target.Size);

        Task = Task.Garrison;
        m_Agent.stoppingDistance = targetSize;
        ServerMove(target.transform.position, true);
    }

    [Server]
    private IEnumerator UpdateTargetPosition()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        Attack(true);
    }

    #endregion

    [ClientRpc]
    private void ClientDebug(string message)
    {
        Debug.Log(message);
    }
}