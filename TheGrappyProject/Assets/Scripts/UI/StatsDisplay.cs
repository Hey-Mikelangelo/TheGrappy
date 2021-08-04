using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class StatsDisplay : MonoBehaviour
{
    [Inject] private GameEventsSO gameEvents;
    [Inject] private GameProgressManager progressManager;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI scoreText;
    public Color collectedScoreColor;
    private Color _initScoreColor;
    private Coroutine _shineScoreCoroutine;
    private void OnEnable()
    {
        gameEvents.onCollected += OnCollected;
        _initScoreColor = scoreText.color;
    }
    private void OnDisable()
    {
        gameEvents.onCollected -= OnCollected;

    }
    void Update()
    {
        scoreText.text = progressManager.CurrentGameData.Score.ToString();
    }
    void OnCollected(Collectible col)
    {
        if(col == Collectible.coin)
        {
            if(_shineScoreCoroutine != null)
            {
                StopCoroutine(_shineScoreCoroutine);
            }
            _shineScoreCoroutine = StartCoroutine(ToYellowAndBack(0.2f, 0.2f));
        }
    }
    IEnumerator ToYellowAndBack(float timeTo, float timeFrom)
    {
        float timeElapsed = 0;
        Color color;
        while(timeElapsed < timeTo)
        {
            timeElapsed += Time.deltaTime;
            color = Color.Lerp(_initScoreColor, collectedScoreColor, timeElapsed / timeTo);
            scoreText.color = color;
            yield return new WaitForFixedUpdate();
        }
        timeElapsed = 0;
        while (timeElapsed < timeTo)
        {
            timeElapsed += Time.deltaTime;
            color = Color.Lerp(collectedScoreColor, _initScoreColor, timeElapsed / timeFrom);
            scoreText.color = color;
            yield return new WaitForFixedUpdate();
        }
        scoreText.color = _initScoreColor;

    }
}
