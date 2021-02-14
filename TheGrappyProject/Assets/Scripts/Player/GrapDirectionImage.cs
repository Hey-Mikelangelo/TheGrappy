using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class GrapDirectionImage : MonoBehaviour
{
    public PlayerVarsSO playerVars;

    private SpriteRenderer image;
    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (playerVars.hasGrapPoint && playerVars.currentMovePhase == MovePhase.aim)
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }
    }
}
