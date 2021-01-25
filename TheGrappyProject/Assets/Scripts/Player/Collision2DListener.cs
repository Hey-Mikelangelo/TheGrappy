using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision2DListener : MonoBehaviour
{
    [SerializeField] private Collision2DEventChannelSO playerCollisionChannel;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerCollisionChannel.Call(gameObject, collision);
    }
}
