using Mirror;
using UnityEngine;

public class ResourceGenerator : NetworkBehaviour
{
    [SerializeField]
    private int resourcesPerInterval = 10;

    [SerializeField]
    private float interval = 2f;

    private float timer;
    private RtsPlayer player;

    public override void OnStartServer()
    {
        timer = interval;
        player = connectionToClient.identity.GetComponent<RtsPlayer>();
    }

    [Server]
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            var resources = player.GetResources();

            foreach (var resource in resources)
            {
                player.SetResources((int)resource.Key, resourcesPerInterval);
            }

            timer += interval;
        }
    }
}
