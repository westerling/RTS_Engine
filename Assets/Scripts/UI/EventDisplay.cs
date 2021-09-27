using Mirror;
using TMPro;
using UnityEngine;

public class EventDisplay : NetworkBehaviour
{
    [SerializeField]
    private TMP_Text m_ChatText = null;

    private void Start()
    {
        Message.OnMessage += HandleNewMessage;
    }

    private void OnDestroy()
    {
        if (!hasAuthority)
        {
            return;
        }

        Message.OnMessage -= HandleNewMessage;
    }

    private void HandleNewMessage(string message, Color color)
    {
        m_ChatText.text += message;
    }
}
