using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Collision2DEventChannelSO", menuName = "Game/Collision2DEventChannel")]
public class Collision2DEventChannelSO : ScriptableObject
{
    public UnityAction<GameObject, Collision2D> collision2DEvent;

    public void Call(GameObject caller, Collision2D collision)
    {
        collision2DEvent?.Invoke(caller, collision);
    }
}
