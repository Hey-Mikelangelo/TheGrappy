using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesToLoadHolder : MonoBehaviour
{
    public SceneLoaderSO sceneLoader;
    public List<SceneInfoSO> scenesToLoad = new List<SceneInfoSO>(1);

    public void LoadScenes()
    {
        sceneLoader.LoadScenes(scenesToLoad);
    }
    private void Awake()
    {
        sceneLoader.LoadAlwaysLoadScene();
    }
}
