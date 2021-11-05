﻿using UnityEngine.UI;
using UnityEngine;
using Mirror;

public class HealthDisplay : NetworkBehaviour
{
    [SerializeField]
    private Image healthBarImage = null;

    private Health m_Health = null;

    public Health Health
    {
        get => m_Health;
        set => m_Health = value;
    }

    private void Awake()
    {
        m_Health = GetComponent<Health>();

        SetupListeners();
    }

    private void OnDestroy()
    {
        if (Health == null)
        {
            return;
        }

        RemoveListeners();
    }

    public void SetupListeners()
    {
        if (Health == null)
        {
            return;
        }

        Health.EventHealthChanged += RpcHandleHealthChanged;
    }

    public void RemoveListeners()
    {
        if (Health == null)
        {
            return;
        }

        Health.EventHealthChanged -= RpcHandleHealthChanged;
    }

    [ClientRpc]
    private void RpcHandleHealthChanged(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }
}
