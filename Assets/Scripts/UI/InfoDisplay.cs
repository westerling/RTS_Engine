using Mirror;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : NetworkBehaviour
{
    [SerializeField]
    private TMP_Text nameText = null;

    [SerializeField]
    private Image icon = null;

    [SerializeField]
    private TMP_Text healthText = null;

    [SerializeField]
    private TMP_Text m_ResourceAmountText = null;

    [SerializeField]
    private Image m_ResourceIcon = null;

    [SerializeField]
    private Image healthImage = null;

    [SerializeField]
    private Image researchImage = null;

    [SerializeField]
    private GameObject infoPanel = null;

    [SerializeField]
    private GameObject statsPanel = null;

    [SerializeField]
    private GameObject m_ResourcePanel = null;

    [SerializeField]
    private GameObject researchPanel = null;

    [SerializeField]
    private GameObject armyPanel = null;

    [SerializeField]
    private GameObject[] armyPanelPrefabs = null;

    private List<Health> healthList = new List<Health>();
    private RtsPlayer player;
    private float progressImageVelocity;

    void Start()
    {
        player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
    }

    private void UpdateUnitInfo(Unit unit)
    {
        infoPanel.SetActive(true);
        statsPanel.SetActive(true);
        researchPanel.SetActive(false);

        icon.sprite = unit.Icon;
        nameText.text = unit.Name;

        switch (unit.UnitMovement.Task)
        {
            case Task.Collect:
            case Task.Deliver:
                m_ResourcePanel.SetActive(true);
                m_ResourceAmountText.text = unit.Collector.CarryingAmount.ToString();
                switch (unit.Collector.Resource)
                {
                    case Resource.Food:
                        nameText.text = "Forager";
                        break;
                    case Resource.Wood:
                        nameText.text = "Lumberjack";
                        break;
                    case Resource.Stone:
                        nameText.text = "Stone Miner";
                        break;
                    case Resource.Gold:
                        nameText.text = "GoldMiner";
                        break;
                }
                break;
            default:
                m_ResourcePanel.SetActive(false);
                break;
        }


        healthList.Add(unit.GetComponent<Health>());

        return;
    }

    private void UpdateArmyInfo(List<Unit> units)
    {
        infoPanel.SetActive(true);
        armyPanel.SetActive(true);

        ClearArmyIcons();

        for (int i = 0; i < units.Count; i++)
        {
            armyPanelPrefabs[i].SetActive(true);

            var icon = armyPanelPrefabs[i].GetComponent<Image>();

            icon.sprite = units[i].Icon;
        }
    }

    private void UpdateHealthDisplay()
    {
        foreach (var health in healthList)
        {
            health.EventHealthChanged += RpcHandleHealthChanged;
        }
    }

    public void ClearArmyIcons()
    {
        foreach (var panel in armyPanelPrefabs)
        {
            panel.gameObject.SetActive(false);
        }
    }

    private void UpdateBuildingInfo(Building building)
    {
        infoPanel.SetActive(true);
        statsPanel.SetActive(true);

        icon.sprite = building.Icon;
        nameText.text = building.Name;

        healthList.Add(building.GetComponent<Health>());

        if (building.TryGetComponent(out Spawner unitSpawner))
        {
            if (unitSpawner.SpawnQue.Count > 0)
            {
                researchPanel.SetActive(true);

                var unitTimer = unitSpawner.ResearchTimer;
                var unitSpawnTime = unitSpawner.SpawnTime;
                UpdateTimerDisplay(unitTimer, unitSpawnTime);
            }
            else
            {
                researchPanel.SetActive(false);
            }
        }

        return;
    }

    private void UpdateTimerDisplay(float unitTimer, float unitSpawnTime)
    {
        var newProgress = unitTimer / unitSpawnTime;

        if (researchImage == null)
        {
            return;
        }

        if (newProgress < researchImage.fillAmount)
        {
            researchImage.fillAmount = newProgress;
        }
        else
        {
            researchImage.fillAmount = Mathf.SmoothDamp(researchImage.fillAmount, newProgress, ref progressImageVelocity, 0.1f);
        }
    }

    private void ClientHandleSelectedGameObject(List<GameObject> obj)
    {
        infoPanel.SetActive(false);
        statsPanel.SetActive(false);
        researchPanel.SetActive(false);
        armyPanel.SetActive(false);

        foreach (var health in healthList)
        {
            health.EventHealthChanged -= RpcHandleHealthChanged;
        }

        healthList.Clear();

        if (obj == null || obj.Count == 0)
        {
            NoneSelected();
            return;
        }

        if (obj.Count == 1)
        {
            SingleSelection(obj[0]);
        }

        if (obj.Count > 1)
        {
            MultiSelection(obj);
        }
    }

    private void NoneSelected()
    {
        infoPanel.SetActive(false);
        statsPanel.SetActive(false);
        researchPanel.SetActive(false);
        armyPanel.SetActive(false);
    }

    private void MultiSelection(List<GameObject> obj)
    {
        var selectedUnits = new List<Unit>();

        foreach (var entity in obj)
        {
            if (entity.TryGetComponent(out Unit unit))
            {
                selectedUnits.Add(unit);
            }
        }

        if (selectedUnits.Count == 0)
        {
            return;
        }

        UpdateArmyInfo(selectedUnits);
    }

    private void SingleSelection(GameObject selectedGameObject)
    {
        if (selectedGameObject.TryGetComponent(out Building building))
        {
            UpdateBuildingInfo(building);
        }
        else if (selectedGameObject.TryGetComponent(out Unit unit))
        {
            UpdateUnitInfo(unit);
        }
        else if (selectedGameObject.TryGetComponent(out Collectable collectable))
        {
            UpdateUnitInfo(unit);
        }

        UpdateHealthDisplay();
    }

    [ClientRpc]
    private void RpcHandleHealthChanged(int currentHealth, int maxHealth)
    {
        healthImage.fillAmount = (float)currentHealth / maxHealth;

        healthText.text = currentHealth + "/" + maxHealth;
    }
}
