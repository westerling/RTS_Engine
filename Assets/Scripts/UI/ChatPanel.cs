using Mirror;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ChatPanel : NetworkBehaviour
{
    [SerializeField]
    private TMP_InputField m_InputField = null;

    [SerializeField]
    private GameObject m_PopupPanel = null;

    [SerializeField]
    private GameObject m_Popup = null;

    private const string InputChat = "Chat";
    private const string InputSend = "Send";

    public static event Action<string, Color, float> OnMessage;

    public static ChatPanel Current;

    private void Start()
    {
        Current = this;

        InputManager.Current.Controls.actions[InputChat].performed += ChatPerformed;
        InputManager.Current.Controls.actions[InputSend].performed += SendPerformed;

        OnMessage += AddMessageToBoard;
    }

    private void ChatPerformed(InputAction.CallbackContext obj)
    {
        InputManager.Current.SetContext(GameContext.Menu);

        EventSystem.current.SetSelectedGameObject(m_InputField.gameObject);
        m_InputField.ActivateInputField();
        m_InputField.gameObject.SetActive(true);
    }

    private void SendPerformed(InputAction.CallbackContext obj)
    {
        InputManager.Current.SetContext(GameContext.Normal);

        Send(m_InputField.text);
        m_InputField.DeactivateInputField();
        m_InputField.gameObject.SetActive(false);
    }

    [Client]
    public void Send(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        var color = NetworkClient.connection.identity.GetComponent<RtsPlayer>().TeamColor;

        CmdSendMessage(message, color, 3f);

        m_InputField.text = string.Empty;
    }

    [Command]
    private void CmdSendMessage(string message, Color color, float duration)
    {
        RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}", color, duration);
    }

    [ClientRpc]
    private void RpcHandleMessage(string message, Color color, float duration)
    {
        OnMessage?.Invoke($"\n{message}", color, duration);
    }

    public void AddMessageToBoard(string message)
    {
        AddMessageToBoard(message, Color.white, 5f);
    }

    public void AddMessageToBoard(string message, Color color, float duration)
    {
        var popup = Instantiate(m_Popup);
        popup.transform.parent = m_PopupPanel.transform;
        popup.GetComponent<Popup>().CreatePopup(message, color, duration);

        if (m_PopupPanel.transform.childCount > 10)
        {
            Destroy(m_PopupPanel.transform.GetChild(0).gameObject);
        }
    }

    private void OnDestroy()
    {
        if (InputManager.Current == null)
        {
            return;
        }

        InputManager.Current.Controls.actions[InputChat].performed -= ChatPerformed;
        InputManager.Current.Controls.actions[InputSend].performed -= SendPerformed;
    }
}
