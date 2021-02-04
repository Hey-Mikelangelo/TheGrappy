using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Game/GameDataSO")]
public class GameDataSO : ScriptableObject
{
    public List<AchievementSO> Achievements = new List<AchievementSO>();
    public List<SkinSO> Skins = new List<SkinSO>();
    public StoreSO store;

}
