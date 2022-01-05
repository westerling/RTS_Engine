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
        
    }

    [ServerCallback]
    private void Update()
    {
        StartCoroutine(Move());
        m_Timer -= Time.deltaTime;
    }

    [Server]
    public IEnumerator Move()
    {
        yield return new WaitUntil(() => m_Timer <= 0);

        var randTie = Random.Range(1, 10);

        m_Timer = randTie;

        var newPos = Utils.OffsetPoint(transform.position, 5);

        m_UnitMovement.ServerMove(newPos);
    }
}
