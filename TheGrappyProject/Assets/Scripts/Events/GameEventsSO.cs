using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEventsSO", menuName = "Game/GameEvents")]
public class GameEventsSO : ScriptableObject
{
    public event UnityAction onPressPlay;
    public event UnityAction<float> onUnfadeToPlay;
    public event UnityAction onUnfadedToPlay;
    public event UnityAction<float> onFadeToMenu;
    public event UnityAction onFadedToMenu;
    public event UnityAction onPlayerDeath;
    public event UnityAction onMapLoaded;
    public event UnityAction<Ability> onChangedAbility;
    public event UnityAction onMapGenerated;
    public event UnityAction onSetIsDestroyerFalse;
    public event UnityAction<Collectible> onCollected;
    public void OnPressPlay()
    {
        onPressPlay?.Invoke();
    }
    public void OnUnfadedToPlay()
    {
        onUnfadedToPlay?.Invoke();
    }
    public void OnUnfadeToPlay(float time)
    {
        onUnfadeToPlay?.Invoke(time);
    }
    public void OnFadeToMenu(float time)
    {
        onFadeToMenu?.Invoke(time);
    }
    public void OnFadedToMenu() 
    {
        onFadedToMenu?.Invoke();
    }
    public void OnPlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }
    public void OnMapLoaded()
    {
        onMapLoaded?.Invoke();
    }
    public void OnChangedAbility(Ability ability)
    {
        onChangedAbility?.Invoke(ability);
    }
    public void OnMapGenerated()
    {
        onMapGenerated?.Invoke();
    }
    public void OnSetIsDestroyerFalseDelayed()
    {
        onSetIsDestroyerFalse?.Invoke();
    }
    public void OnCollected(Collectible col)
    {
        onCollected?.Invoke(col);
    }
}
