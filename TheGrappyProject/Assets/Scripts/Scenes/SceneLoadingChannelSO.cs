using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoadingChannelSO", menuName = "Scene Data/SceneLoadingChannel")]
public class SceneLoadingChannelSO : ScriptableObject
{
    public SceneInfoSO InitScene;
    public UnityAction<List<SceneInfoSO>, bool> onLoadScenes;
    public UnityAction<int> onSceneInted;
    public UnityAction onQuit;
    public UnityAction<SceneTransitionSO> onSetSceneTransition;
    private static bool _loadedInitScene;
    public void Load(List<SceneInfoSO> Scenes, bool showLoadingScreen)
    {
        onLoadScenes?.Invoke(Scenes, showLoadingScreen);
    }
    public void Quit()
    {
        onQuit?.Invoke();
    }
    public void SetSceneInited(int index)
    {
        onSceneInted?.Invoke(index);
    }
    public void SetSceneTransition(SceneTransitionSO transitionSO)
    {
        onSetSceneTransition?.Invoke(transitionSO);
    }
    private void Awake()
    {
      
    }
    public void LoadInitScene()
    {
        if(!_loadedInitScene){
            SceneManager.LoadScene(InitScene.buildIndex, LoadSceneMode.Additive);
            _loadedInitScene = true;
        }
    }
}
