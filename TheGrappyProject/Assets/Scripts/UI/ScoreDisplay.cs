using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public LinkerSO linker;
    private void OnEnable()
    {
        linker.gameEvents.onPlayerDeath += UpdateScore;
        UpdateScore();
    }
    private void OnDisable()
    {
        linker.gameEvents.onPlayerDeath -= UpdateScore;

    }
    void UpdateScore()
    {
        scoreText.text = linker.playerData.lastScore.ToString();

    }
}
