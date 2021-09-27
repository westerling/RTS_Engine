using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : GameObjectIdentity
{
    [SerializeField]
    private Transform m_AimAtPoint = null;

    [SerializeField]
    private Domain m_Domain = Domain.Ground;

    [SerializeField]
    private UnityEvent onSelected = null;

    [SerializeField]
    private UnityEvent onDeselected = null;

    public Transform AimAtPoint
    {
        get { return m_AimAtPoint; }
    }

    public Domain Domain
    {
        get { return m_Domain; }
    }

    [Client]
    public void Select()
    {
        if (!hasAuthority)
        {
            return;
        }

        onSelected?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if (!hasAuthority)
        {
            return;
        }

        onDeselected?.Invoke();
    }
}
