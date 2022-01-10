using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyDisplay : MonoBehaviour
{
    public static ArmyDisplay Current;

    [SerializeField]
    private GameObject m_ArmyPanel = null;

    [SerializeField]
    private SelectionHandler m_SelectionHandler = null;

    [SerializeField]
    private GameObject m_ButtonPrefab = null;

    [SerializeField]
    private Transform m_ParentTransform;

    private List<GameObject> m_Buttons = new List<GameObject>();
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
        ClearButtons();
    }

    public void ClearButtons()
    {
        for (var i = m_Buttons.Count - 1; i >= 0; i--)
        {
            m_Buttons[i].GetComponent<UIHealthDisplay>().RemoveListeners();
            Destroy(m_Buttons[i].gameObject);
            m_Buttons.RemoveAt(i);
        }

        for (var i = m_UnitList.Count - 1; i >= 0; i--)
        {

            m_UnitList.RemoveAt(i);
        }

        m_ArmyPanel.SetActive(false);
    }

    public void AddButtons(Unit[] unitList)
    {
        for (var i = 0; i < unitList.Length; i++)
        {
            var go = Instantiate(m_ButtonPrefab, m_ParentTransform);
            
            go.name = unitList[i].Name + "_Button";
            go.GetComponent<Image>().sprite = unitList[i].Icon;
            go.GetComponent<UIHealthDisplay>().SetupListeners();

            var button = go.GetComponent<Button>();
            var n = i;

            button.onClick.AddListener(() => OnButtonClick(n));

            m_UnitList.Add(unitList[i]);
            m_Buttons.Add(go);
        }

        m_ArmyPanel.SetActive(true);
    }

    public void OnButtonClick(int index)
    {
        m_SelectionHandler.SelectSingleUnit(m_UnitList[index].GetComponent<Unit>());
    }
}
