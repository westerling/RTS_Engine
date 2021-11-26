using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;



public class HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject m_Popup;

    [SerializeField]
    private TMP_Text m_Text;

    [SerializeField]
    private RectTransform m_BackgroundTransform;

    private string m_Description;

    public string Description 
    { 
        get => m_Description; 
        set => m_Description = value; 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.activeSelf)
        {
            ShowPopup();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HidePopup();
    }

    private void ShowPopup()
    {
        m_Popup.SetActive(true);
        m_Text.SetText(Description);

        var paddingSize = Description.Length > 0 ? 4f : 0f;
        var backgroundSize = new Vector2(256, m_Text.preferredHeight + paddingSize * 2);
        m_BackgroundTransform.sizeDelta = backgroundSize;

        var position = new Vector2(transform.position.x, transform.position.y + (GetComponent<RectTransform>().sizeDelta.y / 2));
        m_Popup.transform.position = position;
    }

    private void HidePopup()
    {
        m_Popup.SetActive(false);
    }
}
