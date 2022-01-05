using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
    private Image m_HealthBarImage = null;
    private Health m_Health = null;

    public Health Health
    {
        get => m_Health;
        set => m_Health = value;
    }

    private void Awake()
    {
        var symbolsManager = NetworkClient.connection.identity.GetComponent<SymbolsManager>();
        var healthBar = Instantiate(symbolsManager.HealthBar, transform);
        healthBar.transform.position = SetHeighth();

        m_HealthBarImage = healthBar.GetComponent<HealthBar>().HealthBarImage;
        m_Health = GetComponent<Health>();


        SetupListeners();
    }

    private Vector3 SetHeighth()
    {
        //var height = GetComponent<Collider>().bounds.size.y;

        return new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
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
        m_HealthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }
}
