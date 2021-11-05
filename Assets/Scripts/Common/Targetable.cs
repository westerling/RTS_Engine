using Mirror;
using UnityEngine;

public abstract class Targetable : Interactable
{
    [SerializeField]
    private Targeter m_Targeter = null;

    [SerializeField]
    private GameObject m_FieldOfView;
    
    private float m_FieldOfViewDistance = 10;
    private bool m_Pacifist = false;

    public abstract void EnemyReaction(GameObject sender);

    public Targeter Targeter
    {
        get { return m_Targeter; }
        set { m_Targeter = value; }
    }

    public GameObject FieldOfView
    {
        get { return m_FieldOfView; }
        set { m_FieldOfView = value; }
    }

    public float FieldOfViewDistance
    {
        get { return m_FieldOfViewDistance; }
    }

    [Command]
    public void CmdSetFOVAvailability(bool enabled)
    {
        ServerSetFov(enabled);
    }

    [Server]
    public void ServerSetFov(bool enabled)
    {
        FieldOfView?.SetActive(enabled);
    }

    public void SetFOVAvailability(bool enabled)
    {
        FieldOfView?.SetActive(enabled);
    }

    public bool Pacifist
    {
        get { return m_Pacifist; }
    }

    public Stats GetLocalStats()
    {
        return GetComponent<LocalStats>().Stats;
    }
}
