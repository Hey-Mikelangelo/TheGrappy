using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventChannelSO", menuName = "Game/VoidEventChannel")]
public class VoidEventChannelSO : ScriptableObject
{
    public UnityEvent voidEvent;

    public void Call()
    {
        voidEvent?.Invoke();
    }
}
