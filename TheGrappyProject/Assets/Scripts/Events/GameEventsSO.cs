using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "GameEventsSO", menuName = "Game/GameEvents")]
public class GameEventsSO : ScriptableObject
{
    public event System.Action onPressPlay;
    public event System.Action onStartMove;
    public event System.Action<Collectible> onCollected;
    public event System.Action<Collectible> onUseAbility;
    public event System.Action<Collectible> onChangedAbility;
    public event System.Action<Collision2D> onCollision;
    public event System.Action onPlayerDeath;
    public event System.Action onFadedToDeathScreen;
    public event System.Action onFadedFromDeathScreen;
    public event System.Action onEndedTimeForRespawn;
    public event System.Action onPressRespawn;
    public event System.Action onPressNewPlay;
    public event System.Action onNewGameSceneLoaded;
    public event System.Action onMapGenerated;
    public event System.Action onMapReady;
    public event System.Action onStartClearMap;
    public event System.Action onMapCleared;
    public event System.Action onSaveProgress;
    public event System.Action onLoadProgress;
    public event System.Action<Vector2Int> onPlayerChangedChunk;
    public event System.Action onStartGameTimer;
    public event System.Action onEndGameTimer;
    public event System.Action onResetGameTimer;
    public event System.Action<GameObject> onEnemyDeath;

}
