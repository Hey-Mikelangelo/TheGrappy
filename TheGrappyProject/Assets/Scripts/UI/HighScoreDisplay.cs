using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class HighScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    [Inject] private GameProgressManager progressManager;
    [Inject] private GameEventsSO gameEvents;

    private void OnEnable()
    {
        gameEvents.onPlayerDeath += UpdateHighScore;
        UpdateHighScore();
    }
    private void OnDisable()
    {
        gameEvents.onPlayerDeath -= UpdateHighScore;

    }

    void UpdateHighScore()
    {
        highScoreText.text = progressManager.PersistentGameData.HighScore.ToString();

    }
}
