using UnityEngine;

public interface IDropOff
{
    Resource Resource { get; set; }

    GameObject gameObject { get; }

    void Deliver(int amount);
}
