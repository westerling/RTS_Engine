using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionsDisplay : MonoBehaviour
{
    public static ActionsDisplay Current;

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    private List<Action> actionCalls = new List<Action>();

    public ActionsDisplay()
    {
        Current = this;
    }

    public List<Button> GetButtons()
    {
        return buttons;
    }

    public void ClearButtons()
    {
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        actionCalls.Clear();
    }

    public void AddButton(Sprite pic, Action onClick, int position)
    {
        var index = actionCalls.Count;

        //if (buttons[position].IsActive())
        //{
        //    return;
        //}

        buttons[index].gameObject.SetActive(true);
        buttons[index].GetComponent<Image>().sprite = pic;
        actionCalls.Add(onClick);
    }

    public void OnButtonClick(int index)
    {
        actionCalls[index]();
    }

    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var index = i;
            buttons[index].onClick.AddListener(delegate ()
            {
                OnButtonClick(index);
            }
            );
        }
        ClearButtons();
    }
}
