using System.Collections.Generic;

[System.Serializable]
public struct PlayerData
{
    public int coinsCount;
    public string nickname;
    public int playerId;
    public List<int> AchievementIds;
    public List<int> SkinsOpenedIds;
    
    public PlayerData(int coinsCount, string nickname, int playerId, 
        List<int> AchievementIds = null, List<int> SkinsOpenedIds = null)
    {
        this.coinsCount = coinsCount;
        this.nickname = nickname;
        this.playerId = playerId;
        this.AchievementIds = AchievementIds;
        this.SkinsOpenedIds = SkinsOpenedIds;
    }
    public PlayerData(PlayerDataSO data)
    {
        coinsCount = data.coinsCount;
        nickname = data.nickname;
        playerId = data.playerId;
        AchievementIds = data.AchievementIds;
        SkinsOpenedIds = data.SkinsOpenedIds;
    }

}
