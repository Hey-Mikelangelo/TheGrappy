using TMPro;
using UnityEngine;
using Zenject;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    [Inject] GameEventsSO gameEvents;
    [Inject] GameProgressManager progressManager;

    private void OnEnable()
    {
        gameEvents.onPlayerDeath += UpdateScore;
        UpdateScore();
    }
    private void OnDisable()
    {
        gameEvents.onPlayerDeath -= UpdateScore;

    }
    void UpdateScore()
    {
        scoreText.text = progressManager.CurrentGameData.Score.ToString();

    }
}
