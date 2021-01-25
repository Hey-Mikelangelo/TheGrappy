using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoadTest : MonoBehaviour
{
    /*public SceneTransitionSO sceneTransition;
    public SceneLoaderSO sceneLoader;
    public SceneInfoSO persistentScene;

    public float loadProgress;
    public List<SceneInfoSO> Scenes = new List<SceneInfoSO>();
    private float _transitionTime;
    private RuntimeAnimatorController animatorController;
    private bool _wasScenesLoaded;
    private void OnEnable()
    {
    }
    public void Load()
    {
        _wasScenesLoaded = false;
        sceneLoader.onScenesLoaded += OnScenesLoaded;
        sceneTransition.onStartTransitionCompleted += OnStartTransitionCompleted;
        AsyncLoader.onNewSceneInitCompleted += OnNewSceneInitCompleted;

        sceneTransition.StartTransition(this);
    }
    void OnStartTransitionCompleted()
    {
        sceneLoader.AddPersistentScene(persistentScene);
        sceneLoader.LoadScenes(Scenes);
        StartCoroutine(ProgressBar());
    }
    public void StartLoadingScreen()
    {
        sceneTransition.StartTransition(this);
    }
    public void EndLoadingScreen()
    {
       // sceneTransition.EndTransition();
    }
    IEnumerator ProgressBar()
    {
        loadProgress = sceneLoader.GetLoadingProgress();
        sceneTransition.SetProgressValue(loadProgress);

        while (loadProgress != 1)
        {
            loadProgress = sceneLoader.GetLoadingProgress();
            sceneTransition.SetProgressValue(loadProgress);
            yield return null;
        }
    }
    void OnScenesLoaded()
    {
        _wasScenesLoaded = true;
    }
    void OnNewSceneInitCompleted()
    {
        StartCoroutine(TryEndLoading());
    }
    IEnumerator TryEndLoading()
    {
        while (!_wasScenesLoaded)
        {
            yield return null;
        }
        //sceneTransition.EndTransition();

    }*/
}
