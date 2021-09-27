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

    private Controls m_Controls;

    private void Start()
    {
        m_Controls = new Controls();
        m_Controls.Player.Chat.performed += GeneralControlsPerformed;
        m_Controls.Enable();
    }

    private void GeneralControlsPerformed(InputAction.CallbackContext obj)
    {
        if (m_InputField.gameObject.activeInHierarchy)
        {
            Send(m_InputField.text);
            m_InputField.DeactivateInputField();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(m_InputField.gameObject);
            m_InputField.ActivateInputField();
        }

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
        m_Controls.Player.Chat.performed -= GeneralControlsPerformed;
    }
}
