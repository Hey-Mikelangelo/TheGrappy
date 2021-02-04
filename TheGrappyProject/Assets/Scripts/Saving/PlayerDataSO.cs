using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Game/PlayerDataSO")]
[System.Serializable]
public class PlayerDataSO : ScriptableObject
{
    public int coinsCount;
    public string nickname;
    public int playerId; 
    public List<int> AchievementIds;
    public List<int> SkinsOpenedIds;

    public void SetData(PlayerData data)
    {
        coinsCount = data.coinsCount;
        nickname = data.nickname;
        playerId = data.playerId;
        AchievementIds = data.AchievementIds;
        SkinsOpenedIds = data.SkinsOpenedIds;
    }

    public static explicit operator PlayerData(PlayerDataSO data)
    {
        return new PlayerData
        {
            coinsCount = data.coinsCount,
            nickname = data.nickname,
            playerId = data.playerId,
            AchievementIds = data.AchievementIds,
            SkinsOpenedIds = data.SkinsOpenedIds
        };
    }
    
}
