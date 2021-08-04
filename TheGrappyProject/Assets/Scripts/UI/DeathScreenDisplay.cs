using TMPro;
using UnityEngine;
using Zenject;

public class DeathScreenDisplay : MonoBehaviour
{
    [Inject] private GameProgressManager progressManager;
    [Inject] private GameEventsSO gameEvents;

    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;

    public void OnEnable()
    {
        UpdateScoreAndHighscore();
        gameEvents.onPlayerDeath += OnDeath;
    }
    public void OnDisable()
    {
        gameEvents.onPlayerDeath -= OnDeath;

    }
    private void OnDeath()
    {
        UpdateScoreAndHighscore();
    }
    private void UpdateScoreAndHighscore()
    {
        score.text = progressManager.CurrentGameData.Score.ToString();
        highScore.text = progressManager.PersistentGameData.HighScore.ToString();
    }
}
