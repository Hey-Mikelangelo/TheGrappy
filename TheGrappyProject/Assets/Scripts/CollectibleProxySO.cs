using UnityEngine;
using UnityEngine.Events;

public class CollectibleProxySO : ScriptableObject
{
    public UnityEvent<Collectible> collectEvent;

    public void Call(Collectible item)
    {
        collectEvent?.Invoke(item);
    }
}
