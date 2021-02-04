using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float timeElapsed;
    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
