using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsSO", menuName = "Game/GameSettingsSO")]
[System.Serializable]
public class GameSettingsSO : ScriptableObject
{
    public float soundVolume;
    public float sfxVolume;
    public bool darkTheme;
}
