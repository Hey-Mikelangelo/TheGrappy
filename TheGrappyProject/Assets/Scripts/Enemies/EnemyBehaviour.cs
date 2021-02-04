using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    public abstract void OnPlayerSpoted(Transform player);
    public abstract void OnPlayerNotInSight();
}
