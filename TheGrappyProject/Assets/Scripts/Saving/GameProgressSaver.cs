using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class GameProgressSaver : MonoBehaviour
{
    public DataManagerSO dataManager;
    private void Start()
    {
        
    }
    public PlayerData GetData() {
        return (PlayerData)dataManager.playerData;
    }
    private void OnDisable()
    {
       // EditorApplication.playModeStateChanged -= LoadSave;
    }
    /*void LoadSave(PlayModeStateChange change)
    {
        if(change == PlayModeStateChange.EnteredPlayMode)
        {
            LoadProgress();
        }
        else if(change == PlayModeStateChange.ExitingPlayMode)
        {
            SaveProgress();
        }
    }*/
    public void SaveProgress()
    {
        /*DataSaver<PlayerData>.Save(
           (PlayerData)dataManager.playerData, 
            "player.data");*/
    }
    public PlayerData LoadProgress()
    {
        /* PlayerData data = DataSaver<PlayerData>.Load("player.data");
         dataManager.playerData.SetData(data);
         return data;*/
        return new PlayerData(10, "Player1", 0);
    }
}
