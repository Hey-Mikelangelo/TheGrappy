using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    public PlayerDataSO playerData;
    public TextMeshProUGUI coinsText;
    private int _prevCoinsCount = 0;
    void Update()
    {
        if(_prevCoinsCount != playerData.coinsCount)
        {
            coinsText.text = "coins: " + playerData.coinsCount;
        }
    }
}
