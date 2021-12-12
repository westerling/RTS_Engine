using System.Collections;
using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_TextBox;
    
    private float m_Timer;

    public void CreatePopup(string message, Color color, float duration)
    {
        m_TextBox.text = message;
        m_TextBox.color = color;
        m_Timer = duration;

        StartCoroutine(Lifetime());
    }

    private void Update()
    {
        m_Timer -= Time.deltaTime;
    }

    public IEnumerator Lifetime()
    {
        yield return new WaitUntil(() => m_Timer <= 0);

        Destroy(gameObject);
    }
}
