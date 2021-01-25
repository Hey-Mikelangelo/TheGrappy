using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesToLoadHolder : MonoBehaviour
{
    public SceneLoadingManagerSO sceneLoadingManager;
    public List<SceneInfoSO> scenesToLoad = new List<SceneInfoSO>(1);

    public void LoadScenes()
    {
        sceneLoadingManager.LoadScenes(scenesToLoad, true);
    }
    private void Awake()
    {
        sceneLoadingManager.LoadPersistentScenes();
    }
}
