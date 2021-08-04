using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
public class CollectibleItem : MonoBehaviour
{
    public Collectible type;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collides with wall
        if (collision.collider.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
  
}
