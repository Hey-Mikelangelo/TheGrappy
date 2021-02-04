using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataManagerSO", menuName = "Game/DataManagerSO")]
public class DataManagerSO : ScriptableObject
{
    public PlayerDataSO playerData;
    public GameSettingsSO gameSettings;
    public GameDataSO gameData;
}
