using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "GameEventsSO", menuName = "Game/GameEvents")]
public class GameEventsSO : ScriptableObject
{
    public event UnityAction onPressPlay;
    public event UnityAction onStartMove;
    public event UnityAction<Collectible> onCollected;
    public event UnityAction<Collectible> onUseAbility;
    public event UnityAction<Collectible> onChangedAbility;
    public event UnityAction<Collision2D> onCollision;
    public event UnityAction onPlayerDeath;
    public event UnityAction onFadedToDeathScreen;
    public event UnityAction onFadedFromDeathScreen;
    public event UnityAction onEndedTimeForRespawn;
    public event UnityAction onPressRespawn;
    public event UnityAction onPressNewPlay;
    public event UnityAction onNewGameSceneLoaded;
    public event UnityAction onMapGenerated;
    public event UnityAction onMapReady;
    public event UnityAction onStartClearMap;
    public event UnityAction onMapCleared;
    public event UnityAction onSaveProgress;
    public event UnityAction onLoadProgress;
    public event UnityAction<Vector2Int> onPlayerChangedChunk;
    public event UnityAction onStartGameTimer;
    public event UnityAction onEndGameTimer;
    public event UnityAction onResetGameTimer;

    public void PressPlay()
    {
        onPressPlay?.Invoke();
    }
    public void StartMove()
    {
        onStartMove?.Invoke();
    }
    public void Collected(Collectible col)
    {
        onCollected?.Invoke(col);
    }
    public void UseAbility(Collectible col)
    {
        onUseAbility?.Invoke(col);
    }
    public void ChangedAbility(Collectible col)
    {
        onChangedAbility?.Invoke(col);
    }
    public void Collision(Collision2D collision)
    {
        onCollision?.Invoke(collision);
    }
    public void PlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }
    public void FadedToDeathScreen()
    {
        onFadedToDeathScreen?.Invoke();
    }
    public void FadedFromDeathScreen()
    {
        onFadedFromDeathScreen?.Invoke();
    }
    public void EndedTimeForRespawn()
    {
        onEndedTimeForRespawn?.Invoke();
    }
    public void PressRespawn()
    {
        onPressRespawn?.Invoke();
    }
    public void PressNewPlay()
    {
        onPressNewPlay?.Invoke();
    }
    public void NewGameSceneLoaded()
    {
        onNewGameSceneLoaded?.Invoke();
    }
    public void MapGenerated()
    {
        onMapGenerated?.Invoke();
    }
    public void MapReady()
    {
        onMapReady?.Invoke();
    }
    public void StartClearMap()
    {
        onStartClearMap?.Invoke();
    }
    public void MapCleared()
    {
        onMapCleared?.Invoke();
    }
    public void SaveProgress()
    {
        onSaveProgress?.Invoke();
    }
    public void LoadProgress()
    {
        onLoadProgress?.Invoke();
    }
    public void PlayerChangedChunk(Vector2Int chunk)
    {
        onPlayerChangedChunk?.Invoke(chunk);
    }
    public void StartGameTimer()
    {
        onStartGameTimer?.Invoke();
    }
    public void EndGameTimer()
    {
        onEndGameTimer?.Invoke();
    }
    public void ResetGameTimer()
    {
        onResetGameTimer?.Invoke();
    }
}
