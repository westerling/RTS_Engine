using Mirror;
using UnityEngine;

public class TeamColorSetter : NetworkBehaviour
{
    [SerializeField]
    private Renderer[] colorRenderers = new Renderer[0];

    [SyncVar(hook = nameof(HandleTeamColorUpdated))]
    private Color teamColor = new Color();

    #region server

    public override void OnStartServer()
    {
        var player = connectionToClient.identity.GetComponent<RtsPlayer>();

        teamColor = player.TeamColor;
    }

    #endregion

    #region client

    private void HandleTeamColorUpdated(Color oldColor, Color newColor)
    {
        foreach (var renderer in colorRenderers)
        {
            renderer.material.SetColor("_Color", newColor);
        }
    }

    #endregion
}
