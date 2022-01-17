using Mirror;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(LocalStats))]
public abstract class InteractableGameEntity : Interactable
{
    [SerializeField]
    private Targeter m_Targeter = null;

    [SerializeField]
    private GameObject m_FieldOfView;

    [SerializeField]
    private LocalStats m_LocalStats;

    [SerializeField]
    private Health m_Health = null;

    private float m_FieldOfViewDistance = 10;
    private bool m_Pacifist = false;
    private List<ActionBehaviour> m_ActionBehaviours = new List<ActionBehaviour>();

    private RtsPlayer m_Player;

    public abstract void Reaction(GameObject sender);

    public abstract void ServerHandleDie();

    public abstract void AddBehaviours();

    public Targeter Targeter 
    { 
        get => m_Targeter; 
        set => m_Targeter = value; 
    }

    public LocalStats LocalStats
    {
        get => m_LocalStats;
        set => m_LocalStats = value;
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

    public Health Health
    {
        get => m_Health;
        set => m_Health = value;
    }

    public RtsPlayer Player 
    { 
        get => m_Player;
        set => m_Player = value;
    }
    public List<ActionBehaviour> ActionBehaviours 
    {
        get => m_ActionBehaviours; 
        set => m_ActionBehaviours = value;
    }

    #region client

    public override void OnStartAuthority()
    {
        Player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        var size = LocalStats.Stats.GetAttributeAmount(AttributeType.LineOfSight);

        FieldOfView.transform.localScale += new Vector3(size, 0, size);

        AddBehaviours();
    }

    #endregion

    #region server

    public override void OnStartServer()
    {
        Health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        Health.ServerOnDie -= ServerHandleDie;
    }

    [Command]
    public void CmdSetFOVAvailability(bool enabled)
    {
        ServerSetFov(enabled);
    }

    public void ServerSetFov(bool enabled)
    {
        FieldOfView?.SetActive(enabled);
    }

    public void SetFOVAvailability(bool enabled)
    {
        FieldOfView?.SetActive(enabled);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out InteractableGameEntity targetable))
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
        if (other.TryGetComponent(out InteractableGameEntity targetable))
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

    #endregion

    protected void AddKillEntityAction()
    {

    }
}
