using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionListener : MonoBehaviour
{
    [SerializeField] private GameEventsSO gameEvents;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameEvents.Collision(collision);
    }
}
