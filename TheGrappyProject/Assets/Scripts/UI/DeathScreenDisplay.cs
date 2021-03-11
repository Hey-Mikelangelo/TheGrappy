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

        linker.gameEvents.onPlayerDeath += OnDeath;
    }
    public void OnDisable()
    {
        linker.gameEvents.onPlayerDeath -= OnDeath;

    }
    void OnDeath()
    {
        score.text = linker.playerData.lastScore.ToString();
        highScore.text = linker.playerData.highScore.ToString();
    }
   
}
