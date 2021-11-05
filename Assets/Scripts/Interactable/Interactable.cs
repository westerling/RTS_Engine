using Mirror;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : GameObjectIdentity
{
    [SerializeField]
    private Transform m_AimAtPoint = null;

    [SerializeField]
    private Domain m_Domain = Domain.Ground;

    [SerializeField]
    private EntitySize m_Size = EntitySize.Normal;

    private GameObject m_SelectionIndicator = null;



    public Transform AimAtPoint
    {
        get { return m_AimAtPoint; }
    }

    public Domain Domain
    {
        get { return m_Domain; }
    }

    public EntitySize Size 
    {
        get => m_Size; 
    }

    [Client]
    public void Select()
    {
        if (!hasAuthority)
        {
            return;
        }

        AddSelectionIndicator();
    }

    [Client]
    public void Deselect()
    {
        if (!hasAuthority)
        {
            return;
        }

        RemoveSelectionIndicator();
    }

    private void AddSelectionIndicator()
    {
        var cursorManager = NetworkClient.connection.identity.GetComponent<CursorManager>();
        var indicator = cursorManager.SelectionIndicators[(int)Size];

        m_SelectionIndicator = Instantiate(indicator, transform);
    }

    private void RemoveSelectionIndicator()
    {
        if (m_SelectionIndicator == null)
        {
            return;
        }

        Destroy(m_SelectionIndicator);
    }

    [Client]
    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    public IEnumerator FlashCoroutine()
    {
        AddSelectionIndicator();

        var spriteRenderer = m_SelectionIndicator.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 4; i++)
        {
            spriteRenderer.enabled = (i % 2 == 0);
            yield return new WaitForSeconds(i % 2 == 0 ? 0.2f : 0.1f);
        }

        RemoveSelectionIndicator();
    }
}
