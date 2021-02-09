﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    public LinkerSO linker;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI scoreText;
    public Color collectedScoreColor;
    private int _prevCoinsCount = 0;
    private int _prevScore = 0;
    private Color _initScoreColor;
    private Coroutine _shineScoreCoroutine;
    private void OnEnable()
    {
        linker.gameEvents.onCollected += OnCollected;
        _initScoreColor = scoreText.color;
    }
    private void OnDisable()
    {
        linker.gameEvents.onCollected -= OnCollected;

    }
    void Update()
    {
        if(_prevCoinsCount != linker.playerData.coinsCount)
        {
            //coinsText.text = "coins: " + playerData.coinsCount;
        }
        if(linker.playerData.lastScore - _prevScore >= 100)
        {

        }
        scoreText.text = linker.playerData.lastScore.ToString();
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
            yield return null;
        }
        timeElapsed = 0;
        while (timeElapsed < timeTo)
        {
            timeElapsed += Time.deltaTime;
            color = Color.Lerp(collectedScoreColor, _initScoreColor, timeElapsed / timeFrom);
            scoreText.color = color;
            yield return null;
        }
        scoreText.color = _initScoreColor;

    }
}
