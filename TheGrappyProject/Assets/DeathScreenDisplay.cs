using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenDisplay : MonoBehaviour
{
    public LinkerSO linker;
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;

    public void OnEnable()
    {
        score.text = linker.playerData.lastScore.ToString();
        highScore.text = linker.playerData.highScore.ToString();

        linker.gameEvents.onFadedToDeathScreen += OnFadedToDeathScreen;
    }
    public void OnDisable()
    {
        linker.gameEvents.onFadedToDeathScreen -= OnFadedToDeathScreen;
        linker.gameEvents.onNewGameSceneLoaded -= OnNewGameSceneLoaded;

    }
    void OnFadedToDeathScreen()
    {

    }
    void OnNewGameSceneLoaded()
    {

    }
   
}
