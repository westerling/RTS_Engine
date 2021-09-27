using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyDisplay : MonoBehaviour
{
    public static ArmyDisplay Current;

    [SerializeField]
    private List<Button> m_Buttons = new List<Button>();

    [SerializeField]
    private GameObject m_ArmyPanel = null;

    [SerializeField]
    private SelectionHandler m_SelectionHandler = null;

    private List<Unit> m_UnitList = new List<Unit>();


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
        for (int i = 0; i < m_Buttons.Count; i++)
        {
            var index = i;
            m_Buttons[index].onClick.AddListener(delegate ()
            {
                OnButtonClick(index);
            }
            );
        }
        ClearButtons();
    }

    public void ClearButtons()
    {
        foreach (var button in m_Buttons)
        {
            button.gameObject.SetActive(false);
            button.GetComponent<HealthDisplay>().RemoveListeners();
        }
        m_UnitList.Clear();
        m_ArmyPanel.SetActive(false);
    }

    public void AddButton(Unit unit)
    {
        var index = m_UnitList.Count;
        m_Buttons[index].gameObject.SetActive(true);
        m_Buttons[index].GetComponent<Image>().sprite = unit.Icon;
        m_Buttons[index].GetComponent<HealthDisplay>().Health = unit.GetComponent<Health>();
        m_Buttons[index].GetComponent<HealthDisplay>().SetupListeners();

        m_UnitList.Add(unit);
        m_ArmyPanel.SetActive(true);
    }

    public void OnButtonClick(int index)
    {
        m_SelectionHandler.SelectSingleUnit(m_UnitList[index].GetComponent<Unit>());
    }
}
