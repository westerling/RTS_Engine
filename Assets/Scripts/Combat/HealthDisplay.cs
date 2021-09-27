using UnityEngine.UI;
using UnityEngine;
using Mirror;

public class HealthDisplay : NetworkBehaviour
{
    [SerializeField]
    private Health m_Health = null;

    [SerializeField]
    private Image healthBarImage = null;

    public Health Health
    {
        get => m_Health;
        set => m_Health = value;
    }

    private void Awake()
    {
        if (Health == null)
        {
            return;
        }

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

        Health.EventHealthChanged += HandleHealthChanged;
    }

    public void RemoveListeners()
    {
        if (Health == null)
        {
            return;
        }

        Health.EventHealthChanged -= HandleHealthChanged;
    }

    [ClientRpc]
    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }
}
