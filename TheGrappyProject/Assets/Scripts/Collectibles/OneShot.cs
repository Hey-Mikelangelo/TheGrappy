using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class OneShot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public void Shoot(Transform shotFromTransform, Vector2 aimDelta)
    {
        float playerAngle = Mathf.Atan2(shotFromTransform.right.y, shotFromTransform.right.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.Atan2(aimDelta.y, aimDelta.x) * Mathf.Rad2Deg;
        float angle = deltaAngle + playerAngle;
        Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);
        Instantiate(bulletPrefab, shotFromTransform.position, rotation);
    }
}
