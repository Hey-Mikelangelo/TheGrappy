using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public LinkerSO linker;
    private void OnEnable()
    {
        linker.gameEvents.onPlayerDeath += UpdateHighScore;
        UpdateHighScore();
    }
    private void OnDisable()
    {
        linker.gameEvents.onPlayerDeath -= UpdateHighScore;

    }
    void UpdateHighScore()
    {
        highScoreText.text = linker.playerData.highScore.ToString();

    }
}
