using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthDisplay : NetworkBehaviour
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
