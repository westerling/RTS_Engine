using Mirror;
using TMPro;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text food = null;

    [SerializeField]
    private TMP_Text gold = null;

    [SerializeField]
    private TMP_Text stone = null;

    [SerializeField]
    private TMP_Text wood = null;

    [SerializeField]
    private TMP_Text m_CurrentPopulation = null;  
    
    [SerializeField]
    private TMP_Text m_MaximumPopulation = null;

    private RtsPlayer player;

    private void Start()
    {
        player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        var resources = player.GetResources();

        foreach (var resource in resources)
        {
            ClientHandleResourcesUpdated(resource.Value, resource.Key);
        }

        player.ClientOnResourcesUpdated += ClientHandleResourcesUpdated;
        player.ClientOnCurrentPopulationUpdated += ClientHandleCurrentPopulationUpdated;
        player.ClientOnMaximumPopulationUpdated += ClientHandleMaximumPopulationUpdated;
    }

    private void OnDestroy()
    {
        player.ClientOnResourcesUpdated -= ClientHandleResourcesUpdated;
        player.ClientOnCurrentPopulationUpdated -= ClientHandleCurrentPopulationUpdated;
        player.ClientOnMaximumPopulationUpdated -= ClientHandleMaximumPopulationUpdated;
    }

    private void ClientHandleResourcesUpdated(int resources, Resource resource)
    {
        switch(resource)
        {
            case Resource.Food:
                food.text = $"{resources}";
                break;
            case Resource.Gold:
                gold.text = $"{resources}";
                break;
            case Resource.Stone:
                stone.text = $"{resources}";
                break;
            case Resource.Wood:
                wood.text = $"{resources}";
                break;
        }
    }

    private void ClientHandleMaximumPopulationUpdated(int newPopulation)
    {
        m_MaximumPopulation.text = $"{newPopulation}";
    }

    private void ClientHandleCurrentPopulationUpdated(int newPopulation)
    {
        m_CurrentPopulation.text = $"{newPopulation}";
    }
}
