using Mirror;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody rigidBody = null;


    private float launchForce = 10f;
    private float destroyAfterSeconds = 5f;
    private float m_DamageToDeal;
    private GameObject m_Sender;

    public float DamageToDeal
    {
        get { return m_DamageToDeal; }
        set { m_DamageToDeal = value; }
    }

    public GameObject Sender
    {
        get { return m_Sender; }
        set { m_Sender = value; }
    }

    private void Start()
    {
        rigidBody.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfterSeconds);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NetworkIdentity networkIdentity))
        {
            if (networkIdentity.connectionToClient == connectionToClient)
            {
                return;
            }

            if (other.TryGetComponent(out Health health))
            {
                health.DealDamage((int)DamageToDeal, (int)AttackStyle.Pierce);
            }

            if (other.TryGetComponent(out Targetable targetable))
            {
                targetable.EnemyReaction(Sender);
            }
        }

        DestroySelf();
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
