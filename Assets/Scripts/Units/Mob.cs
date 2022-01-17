using Mirror;
using UnityEngine;
using System.Collections;

public class Mob : InteractableGameEntity
{

    [SerializeField]
    private UnitMovement m_UnitMovement = null;

    private float m_Timer = 1;

    private StanceType m_Stance = StanceType.Defensive;

    public UnitMovement UnitMovement
    {
        get { return m_UnitMovement; }
    }

    public override void Reaction(GameObject sender)
    {
        StopAllCoroutines();
        Flee();
    }

    private void Flee()
    {
        StopAllCoroutines();
        Move(10);
    }

    [ServerCallback]
    private void Update()
    {
        StartCoroutine(WaitForMove());
        m_Timer -= Time.deltaTime;
    }

    [Server]
    private IEnumerator WaitForMove()
    {
        yield return new WaitUntil(() => m_Timer <= 0);
        Move(5);

        
    }

    [Server]
    private void Move(int distance)
    {
        var idleTime = Random.Range(1, 10);
        var randomDistance = Random.Range((distance / 2), distance);

        m_Timer = idleTime;

        var newPos = Utils.OffsetPoint(transform.position, distance);

        m_UnitMovement.ServerMove(newPos);
    }

    public override void ServerHandleDie()
    {
        DestroyThisOnServer();
    }

    public override void AddBehaviours()
    {
        return;
    }
}
