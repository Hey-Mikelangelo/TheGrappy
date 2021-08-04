using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoadingChannelSO", menuName = "Scene Data/SceneLoadingChannel")]
public class SceneLoadingChannelSO : ScriptableObject
{
    public SceneInfoSO InitScene;
    public System.Action<List<SceneInfoSO>, bool> onLoadScenes;
    public System.Action<int> onSceneInted;
    public System.Action onQuit;
    public System.Action onScenesAllOk;
    public System.Action<SceneTransitionSO> onSetSceneTransition;
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

    public void SetInitSceneLoaded()
    {
        _loadedInitScene = true;
    }

    public void LoadInitScene()
    {
        if(!_loadedInitScene){
            SceneManager.LoadScene(InitScene.buildIndex, LoadSceneMode.Additive);
            _loadedInitScene = true;
        }
    }
}
