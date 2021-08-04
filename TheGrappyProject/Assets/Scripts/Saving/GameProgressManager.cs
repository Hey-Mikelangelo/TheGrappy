using UnityEngine;

public class GameProgressManager
{
    public CurrentGameData CurrentGameData => (CurrentGameData)currentGameData.Clone();
    public PersistentGameData PersistentGameData => (PersistentGameData)persistentGameData.Clone();

    private CurrentGameData currentGameData = new CurrentGameData();
    private PersistentGameData persistentGameData = new PersistentGameData();

    private const string SAVE_FILE_NAME = "playerPersistentData";
    
    public void Init()
    {
        ResetCurrentGameData();
        LoadGamePersistentData();
    }
    
    public void Finish()
    {
        SaveGamePersistentData();
    }

    public void UpdateTick()
    {
        currentGameData.TimeSurvived += Time.deltaTime;
    }

    public float GetTimeSurvived()
    {
        return currentGameData.TimeSurvived;
    }

    public void SaveGamePersistentData()
    {
        ObjectSaver<PersistentGameData>.SaveObject(persistentGameData, SAVE_FILE_NAME);
    }

    public void LoadGamePersistentData()
    {
        persistentGameData = ObjectSaver<PersistentGameData>.LoadObject(SAVE_FILE_NAME);
    }

    private void ResetCurrentGameData()
    {
        currentGameData.TimeSurvived = 0;
        currentGameData.CollectedThings = new System.Collections.Generic.Dictionary<Collectible, int>();
    }

}
