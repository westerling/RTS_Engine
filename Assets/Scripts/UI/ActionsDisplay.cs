using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ActionsDisplay : MonoBehaviour
{
    public static ActionsDisplay Current;

    [SerializeField]
    private List<Button> m_ActionButtons = new List<Button>();

    [SerializeField]
    private GameObject m_PanelOne;
    
    [SerializeField]
    private GameObject m_PanelTwo;

    private List<Action> m_ActionCalls = new List<Action>();

    public ActionsDisplay()
    {
        Current = this;
    }

    public void ClearButtons()
    {
        foreach (var button in m_ActionButtons)
        {
            button.gameObject.SetActive(false);
        }

        m_ActionCalls.Clear();
    }

    public void AddButton(Sprite pic, Action onClick)
    {
        var index = m_ActionCalls.Count;

        m_ActionButtons[index].gameObject.SetActive(true);
        m_ActionButtons[index].GetComponent<Image>().sprite = pic;
        m_ActionCalls.Add(onClick);
    }

    public void FillButtons(List<ActionButton> actionButtons)
    {
        for (int i = 0; i < 30; i++)
        {
            if (actionButtons.Any(b => b.Position == i))
            {
                var behaviour = actionButtons.Where(b => b.Position == i).First();

                AddButton(behaviour.Sprite, behaviour.ClickAction);
            }
            else
            {
                var emptyButton = new ActionButton();
                AddButton(emptyButton.Sprite, emptyButton.ClickAction);
            }
        }
    }

    public void ResetPanels()
    {
        m_PanelOne.SetActive(true);
        m_PanelTwo.SetActive(false);
    }

    public void TogglePanels()
    {
        m_PanelOne.SetActive(!m_PanelOne.activeSelf);
        m_PanelTwo.SetActive(!m_PanelTwo.activeSelf);
    }

    public void OnButtonClick(int index)
    {
        m_ActionCalls[index]();
    }

    private void Start()
    {
        for (int i = 0; i < m_ActionButtons.Count; i++)
        {
            var index = i;
            m_ActionButtons[index].onClick.AddListener(delegate ()
            {
                OnButtonClick(index);
            }
            );
        }
        ClearButtons();
    }
}
