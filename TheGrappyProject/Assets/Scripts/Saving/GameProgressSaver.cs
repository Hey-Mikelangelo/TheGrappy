//using UnityEditor;
using UnityEngine;

public class GameProgressSaver : MonoBehaviour
{
    public DataManagerSO dataManager;
    public string saveFileName = "player.data";
    private string coins = "coins";
    private string highScore = "highScore";
    private string playerName = "playerName";

    private void Awake()
    {
        /*if (!Directory.Exists(SerializationManager.GetSavePath()))
        {
            Directory.CreateDirectory(SerializationManager.GetSavePath());
        }
        string path = SerializationManager.GetDefaultFilePath(saveFileName);

        if (File.Exists(path))
        {
            SerializationManager.Save(saveFileName, (PlayerData)dataManager.playerData);
        }*/
    }
    private void Start()
    {

    }

    private void OnDisable()
    {
       // EditorApplication.playModeStateChanged -= LoadSave;
    }
   
    public void SaveProgress()
    {
        //SerializationManager.Save(saveFileName, (PlayerData)dataManager.playerData);
        PlayerPrefs.SetInt(coins, dataManager.playerData.coinsCount);
        PlayerPrefs.SetString(playerName, dataManager.playerData.nickname);
        PlayerPrefs.SetInt(highScore, dataManager.playerData.highScore);
    }
    public void LoadProgress()
    {
        dataManager.playerData.coinsCount = PlayerPrefs.GetInt(coins, 0);
        dataManager.playerData.nickname = PlayerPrefs.GetString(playerName, "player");
        dataManager.playerData.highScore = PlayerPrefs.GetInt(highScore, 0);
        // dataManager.playerData.SetData((PlayerData)SerializationManager.Load(saveFileName));

    }
}
