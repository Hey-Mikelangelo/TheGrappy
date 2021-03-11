//using UnityEditor;
using UnityEngine;

public class GameProgressSaver : MonoBehaviour
{
    public DataManagerSO dataManager;
    public string saveFileName = "player.data";
    private string coins = "coins";
    private string highScore = "highScore";
    private string playerName = "playerName";
    private string lastScore = "lastScore";

    private void Awake()
    {
       
    }
    private void Start()
    {

    }

    private void OnDisable()
    {
    }
   
    public void SaveProgress()
    {
        PlayerPrefs.SetInt(coins, dataManager.playerData.coinsCount);
        PlayerPrefs.SetString(playerName, dataManager.playerData.nickname);
        PlayerPrefs.SetInt(highScore, dataManager.playerData.highScore);
        PlayerPrefs.SetInt(lastScore, dataManager.playerData.lastScore);
    }
    public void LoadProgress()
    {
        dataManager.playerData.coinsCount = PlayerPrefs.GetInt(coins, 0);
        dataManager.playerData.nickname = PlayerPrefs.GetString(playerName, "player");
        dataManager.playerData.highScore = PlayerPrefs.GetInt(highScore, 0);
        dataManager.playerData.lastScore = PlayerPrefs.GetInt(lastScore, 0);

    }
}
