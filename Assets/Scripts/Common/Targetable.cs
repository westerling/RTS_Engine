using Mirror;
using UnityEngine;

public abstract class Targetable : Interactable
{
    [SerializeField]
    private Targeter m_Targeter = null;

    [SerializeField]
    private GameObject m_FieldOfView;

    [SerializeField]
    private Collider m_FieldOfViewCollider = null;
    
    private float m_FieldOfViewDistance = 10;
    private bool m_Pacifist = false;

    public abstract void Reaction(GameObject sender);

    public Targeter Targeter
    {
        get => m_Targeter;
        set => m_Targeter = value;
    }

    public GameObject FieldOfView
    {
        get => m_FieldOfView;
        set => m_FieldOfView = value;
    }

    public float FieldOfViewDistance
    {
        get => m_FieldOfViewDistance;
    }

    public bool Pacifist
    {
        get => m_Pacifist;
    }

    #region server

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");

        if (other.TryGetComponent(out Targetable targetable))
        {
            if (targetable.hasAuthority)
            {
                return;
            }
            var go = other.gameObject;

            ToggleGameobject(go, true);
            Targeter.SetTarget(targetable.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Fucked off");
        if (other.TryGetComponent(out Targetable targetable))
        {
            if (targetable.hasAuthority)
            {
                return;
            }

            var go = other.gameObject;

            ToggleGameobject(go, false);
        }
    }

    private void ToggleGameobject(GameObject go, bool enabled)
    {
        var renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in renderers)
        {
            renderer.enabled = enabled;
        }
    }

    public Stats GetLocalStats()
    {
        return GetComponent<LocalStats>().Stats;
    }
    #endregion
}
