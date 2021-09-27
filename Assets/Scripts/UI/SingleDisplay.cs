using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class SingleDisplay : NetworkBehaviour
{
    public static SingleDisplay Current;

    [SerializeField]
    private GameObject m_SinglePanel = null;

    [SerializeField]
    private GameObject m_ResourcePanel = null;

    [SerializeField]
    private GameObject m_ResearchPanel = null;

    [SerializeField]
    private TMP_Text m_NameText = null;

    [SerializeField]
    private Image m_Icon = null;

    [SerializeField]
    private Image m_HealthBarImage = null;

    [SerializeField]
    private TMP_Text m_HealthText = null;

    [SerializeField]
    private TMP_Text m_AttackText = null;

    [SerializeField]
    private TMP_Text m_DefenceText = null;

    [SerializeField]
    private TMP_Text m_ResourceAmountText = null;

    [SerializeField]
    private Image m_ResourceIcon = null;

    [SerializeField]
    private List<Button> m_ResearchButtons = new List<Button>();

    [SerializeField]
    private TMP_Text m_CompletionText = null;

    [SerializeField]
    private Image m_CompletionBar = null;

    private GameObject m_SelectedGameObject = null;
    private Health m_Health = null;
    private Collector m_Collector = null;
    private float progressImageVelocity;

    private void Awake()
    {
        if (Current != null && Current != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Current = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < m_ResearchButtons.Count; i++)
        {
            var index = i;
            m_ResearchButtons[index].onClick.AddListener(delegate ()
            {
                OnButtonClick(index);
            }
            );
        }
        ClearInfo();
    }

    public void ClearInfo()
    {
        foreach (var b in m_ResearchButtons)
        {
            b.gameObject.SetActive(false);
        }
        m_SelectedGameObject = null;
        m_SinglePanel.SetActive(false);

        if (m_Health == null)
        {
            return;
        }

        m_Health.EventHealthChanged -= HandleHealthChanged;
        
        if (m_Collector is null)
        {
            return;
        }
        m_Collector.ResourceCollected -= HandleResourceCollected;
    }

    public void AddEntity(GameObject go)
    {
        m_SinglePanel.SetActive(true);
        if (go.TryGetComponent(out Building building))
        {
            UpdateBuildingInfo(building);
        }
        else if (go.TryGetComponent(out Unit unit))
        {
            UpdateUnitInfo(unit);
        }
        else if (go.TryGetComponent(out Collectable collectable))
        {
            UpdateUnitInfo(unit);
        }
    }

    private void UpdateBuildingInfo(Building building)
    {
        SetupBasicInfo(building.Icon, building.Name, building.GetComponent<Health>());
        SetupHealth(building.GetComponent<Health>());

        if (building.TryGetComponent(out Spawner unitSpawner))
        {
            if (unitSpawner.SpawnQue.Count > 0)
            {
                m_ResearchPanel.SetActive(true);

                var unitTimer = unitSpawner.ResearchTimer;
                var unitSpawnTime = unitSpawner.SpawnTime;
                UpdateTimerDisplay(unitTimer, unitSpawnTime);
            }
            else
            {
                m_ResearchPanel.SetActive(false);
            }
        }

        return;
    }

    private void UpdateUnitInfo(Unit unit)
    {
        SetupBasicInfo(unit.Icon, unit.Name, unit.GetComponent<Health>());
        SetupHealth(unit.GetComponent<Health>());

        if (unit.TryGetComponent(out Collector collector))
        {
            SetupCollector(collector);
        }

        switch (unit.UnitMovement.Task)
        {
            case Task.Collect:
            case Task.Deliver:
                m_ResourcePanel.SetActive(true);
                UpdateResourceAmountText(unit.Collector.CarryingAmount);
                switch (unit.Collector.Resource)
                {
                    case Resource.Food:
                        m_NameText.text = "Forager";
                        break;
                    case Resource.Wood:
                        m_NameText.text = "Lumberjack";
                        break;
                    case Resource.Stone:
                        m_NameText.text = "Stone Miner";
                        break;
                    case Resource.Gold:
                        m_NameText.text = "GoldMiner";
                        break;
                }
                break;
            default:
                m_ResourcePanel.SetActive(false);
                break;
        }
        return;
    }

    private void UpdateResourceAmountText(int amount)
    {
        m_ResourceAmountText.text = amount.ToString();
    }

    private void UpdateCollectable(Collectable collectable)
    {
        SetupBasicInfo(collectable.Icon, collectable.Name, collectable.GetComponent<Health>());
        
        return;
    }

    private void UpdateTimerDisplay(float unitTimer, float unitSpawnTime)
    {
        var newProgress = unitTimer / unitSpawnTime;

        if (m_ResearchButtons == null)
        {
            return;
        }

        if (newProgress < m_CompletionBar.fillAmount)
        {
            m_CompletionBar.fillAmount = newProgress;
        }
        else
        {
            m_CompletionBar.fillAmount = Mathf.SmoothDamp(m_CompletionBar.fillAmount, newProgress, ref progressImageVelocity, 0.1f);
        }
    }

    private void SetupBasicInfo(Sprite icon, string name, Health health)
    {
        m_Icon.sprite = icon;
        m_NameText.text = name;
        SetupHealth(health.CurrentHealth, health.MaxHealth);
    }

    private void SetupHealth(Health health)
    {
        m_Health = health;

        health.EventHealthChanged += HandleHealthChanged;
    }

    public void SetupCollector(Collector collector)
    {
        m_Collector = collector;

        collector.ResourceCollected += HandleResourceCollected;
    }

    private void OnButtonClick(int index)
    {
        m_SelectedGameObject.GetComponent<Spawner>().CmdRemoveFromQueue(index);
    }

    private void SetupHealth(int currentHealth, int maxHealth)
    {
        m_HealthBarImage.fillAmount = (float)currentHealth / maxHealth;
        m_HealthText.text = currentHealth + "/" + maxHealth;
    }

    [ClientRpc]
    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        SetupHealth(currentHealth, maxHealth);
    }

    private void HandleResourceCollected(int amount)
    {
        UpdateResourceAmountText(amount);
    }
}
