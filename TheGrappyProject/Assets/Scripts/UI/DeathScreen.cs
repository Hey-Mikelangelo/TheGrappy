using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    bool isDead;
    public GameEventsSO gameEvents;
    public SceneLoadingChannelSO sceneLoadingChannel;
    public List<SceneInfoSO> GameScenes;
    public SceneTransitionSO DeathScreenTransition;
    public SceneTransitionSO HideLoadingSceneTransition;
    private GameObject DeathScreenCanvas;
    private event System.Action onFadedToDeathScreen;

    private void OnEnable()
    {
        gameEvents.onPlayerDeath += OnPLayerDeath;
        gameEvents.onPressRespawn += OnPressRespawn;
        gameEvents.onFadedToDeathScreen += OnFadedToDeathScreen;
       //gameEvents.onNewGameSceneLoaded += OnNewGameSceneLoaded;
        DeathScreenTransition.onStartTransitionCompleted += OnDeathScreenStartTransitionCompleted;
        HideLoadingSceneTransition.onStartTransitionCompleted += OnHideLoadingSceneTransitionCompleted;
        sceneLoadingChannel.onScenesAllOk += OnNewGameSceneLoaded;

    }
    private void OnDisable()
    {
        gameEvents.onPlayerDeath -= OnPLayerDeath;
        gameEvents.onPressRespawn -= OnPressRespawn;
        onFadedToDeathScreen -= OnFadedToDeathScreen;
        DeathScreenTransition.onStartTransitionCompleted -= OnDeathScreenStartTransitionCompleted;
        HideLoadingSceneTransition.onStartTransitionCompleted -= OnHideLoadingSceneTransitionCompleted;
        sceneLoadingChannel.onScenesAllOk -= OnNewGameSceneLoaded;
    }
    
   
    void OnDeathScreenStartTransitionCompleted()
    {
        if (!isDead)
        {
            return;
        }
        onFadedToDeathScreen?.Invoke();
    }
    void OnPLayerDeath()
    {
        isDead = true;
        Scene persistentScene = gameObject.scene;
        DeathScreenCanvas = DeathScreenTransition.StartTransition(this, persistentScene);
        DeathScreenCanvas.GetComponent<CanvasGroup>().interactable = true;
    }
    void OnPressRespawn()
    {
        if (!isDead)
        {
            return;
        }
        DeathScreenCanvas.GetComponent<CanvasGroup>().interactable = false;
        sceneLoadingChannel.SetSceneTransition(HideLoadingSceneTransition);
        sceneLoadingChannel.Load(GameScenes, true);
    }
    void OnNewGameSceneLoaded()
    {
        if (!isDead)
        {
            return;
        }
        isDead = false;
        DeathScreenCanvas.GetComponent<Canvas>().enabled = false;
        DeathScreenTransition.EndTransition();
    }
    void OnFadedToDeathScreen()
    {

    }
    void OnHideLoadingSceneTransitionCompleted()
    {
        
    }
}
