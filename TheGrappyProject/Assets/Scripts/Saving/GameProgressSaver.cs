using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using UnityEditor;
using UnityEngine;

public class GameProgressSaver : MonoBehaviour
{
    public DataManagerSO dataManager;
    public string saveFileName = "player.data";

    private void Awake()
    {
        /*string savePath = DataSaver<int>.InitSaveFolder();
        File.Create(savePath + saveFileName);*/
    }
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
   
    public void SaveProgress()
    {
        SerializationManager.Save(saveFileName, (PlayerData)dataManager.playerData);

    }
    public void LoadProgress()
    {
        dataManager.playerData.SetData((PlayerData)SerializationManager.Load(saveFileName));
      
    }
}
