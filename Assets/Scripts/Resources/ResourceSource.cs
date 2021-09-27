using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class ResourceSource : MonoBehaviour
{
    [SerializeField]
    private Resource resource;

    [SerializeField]
    private int quantity;

    private UnityEvent onQuantityChanged;

    private RtsPlayer player = null;

    private void Start()
    {
        player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();
    }

    public Resource GetResource()
    {
        return resource;
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public void GatherResource(int amount)
    {
        quantity -= amount;

        var amountToGive = amount;

        if (quantity < 0)
        {
            amountToGive = amount + quantity;
        }

        if (quantity <= 0)
        {
            Destroy(gameObject);
        }

        if (onQuantityChanged != null)
        {
            onQuantityChanged.Invoke();
        }
    }
}
