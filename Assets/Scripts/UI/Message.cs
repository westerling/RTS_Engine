using Mirror;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Message : NetworkBehaviour
{
    [SerializeField]
    private TMP_InputField m_InputField = null;

    public static event Action<string, Color> OnMessage;

    private const string InputChat = "Chat";
    private const string InputSend = "Send";

    private void Start()
    {
        InputManager.Current.Controls.actions[InputChat].performed += ChatInputPerformed;
        InputManager.Current.Controls.actions[InputSend].performed += SendInputPerformed;
    }

    private void ChatInputPerformed(InputAction.CallbackContext obj)
    {
                EventSystem.current.SetSelectedGameObject(m_InputField.gameObject);
        m_InputField.ActivateInputField();
        InputManager.Current.SetContext(GameContext.Menu);

        m_InputField.gameObject.SetActive(!m_InputField.gameObject.activeInHierarchy);
    }

    private void SendInputPerformed(InputAction.CallbackContext obj)
    {
        Send(m_InputField.text);
        m_InputField.DeactivateInputField();
        InputManager.Current.SetContext(GameContext.Normal);

        m_InputField.gameObject.SetActive(!m_InputField.gameObject.activeInHierarchy);
    }

    [Client]
    public void Send(string message)
    {
        if (!hasAuthority)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        var color = NetworkClient.connection.identity.GetComponent<RtsPlayer>().TeamColor;

        CmdSendMessage(message, color);

        m_InputField.text = string.Empty;
    }

    [Command]
    private void CmdSendMessage(string message, Color color)
    {
        RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}", color);
    }

    [ClientRpc]
    private void RpcHandleMessage(string message, Color color)
    {
        OnMessage?.Invoke($"\n{message}", color);
    }

    private void OnDestroy()
    {
        InputManager.Current.Controls.actions[InputChat].performed -= ChatInputPerformed;
        InputManager.Current.Controls.actions[InputSend].performed -= SendInputPerformed;
    }
}
