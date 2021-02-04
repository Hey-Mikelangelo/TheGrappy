using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public PlayerManager playerManager;
    public UIFader uiFader;

    private void OnEnable()
    {
        uiFader.onUnfadeCompleted += StartGame;
    }
    private void OnDisable()
    {
        uiFader.onUnfadeCompleted -= StartGame;

    }
    public void StartGame()
    {
        playerManager.runGame = true;
    }
}
