using UnityEngine;

public interface IResource
{
    int Quantity { get; set; }

    Resource Resource { get; }

    GameObject gameObject { get; }

    bool IsInfinite { get; }

    bool CanGather { get; }

    bool GatherResources(int gatherAmount);
}
