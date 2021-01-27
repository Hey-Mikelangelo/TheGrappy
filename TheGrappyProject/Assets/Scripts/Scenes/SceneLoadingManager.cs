using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{
    public SceneLoadingChannelSO sceneLoadingChannelSO;
    
    public List<SceneInfoSO> PersistentScenes = new List<SceneInfoSO>();
    public List<SceneTransitionSO> SceneTransitions = new List<SceneTransitionSO>();
    //Loading scenes
    public List<SceneInfoSO> CurrentLoadingScenes = new List<SceneInfoSO>();
    public Dictionary<int, bool> SceneInitedIndexesDict = new Dictionary<int, bool>();
    public UnityAction<int> onSceneLoaded;

    //Loading screen
    public bool isLoadingScreen;
    public float loadingProgress;

    //Events
    public UnityAction onAllScenesInited;
    public UnityAction onAllScenesLoaded;
    public UnityAction onAllScenesOk;
    public UnityAction onEndTransitionCompleted;

    //Scene transitions
    public SceneTransitionSO currentSceneTransition;

    private int _initedSceneCount;
    private List<SceneInfoSO> _ScenesToLoad;
    private bool _allScenesInited, _allScenesLoaded;

    public void OnEnable()
    {
        sceneLoadingChannelSO.onLoadScenes += LoadScenes;
        sceneLoadingChannelSO.onSceneInted += SetSceneInited;
        sceneLoadingChannelSO.onSetSceneTransition += SetSceneTransition;
        sceneLoadingChannelSO.onQuit += Quit;
        onAllScenesOk += OnAllScenesOk;
        SceneLoader.onScenesLoaded += OnScenesLoaded;

    }
    public void OnDestroy()
    {
        sceneLoadingChannelSO.onLoadScenes -= LoadScenes;
        sceneLoadingChannelSO.onSceneInted -= SetSceneInited;
        sceneLoadingChannelSO.onSetSceneTransition -= SetSceneTransition;
        sceneLoadingChannelSO.onQuit -= Quit;
        onAllScenesOk -= OnAllScenesOk;
        SceneLoader.onScenesLoaded -= OnScenesLoaded;



    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadScenes(List<SceneInfoSO> ScenesToLoad, bool showLoadingScreen)
    {
        SetManager(ScenesToLoad);
        if (showLoadingScreen)
        {
            if (SceneTransitions.Count < 1)
            {
                Debug.LogWarning("No scene transitions in SceneLoadingManagerSO");
                StartLoadingScenes();
            }
            else
            {
                isLoadingScreen = false;
                ShowLoadingScreen();
            }
        }
        else
        {
            isLoadingScreen = false;
            StartLoadingScenes();
        }
    }
    private void LoadPersistentScenes()
    {
        foreach (var scene in PersistentScenes)
        {
            if (SceneManager.GetSceneByBuildIndex(scene.buildIndex).isLoaded)
            {
                continue;
            }
            SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Additive);
            SceneLoader.AddPersistentScene(scene);
        }
    }
    public void SetSceneTransition(SceneTransitionSO transitionSO)
    {
        currentSceneTransition = transitionSO;
    }
    public bool SetSceneTransition(int indexInSceneTranstionList)
    {
        if (indexInSceneTranstionList >= 0 && indexInSceneTranstionList < SceneTransitions.Count)
        {
            currentSceneTransition = SceneTransitions.ElementAt(indexInSceneTranstionList);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void ResetManager()
    {
        loadingProgress = 0;
        SceneInitedIndexesDict.Clear();
        CurrentLoadingScenes.Clear();
        _initedSceneCount = 0;
        _allScenesInited = false;
        _allScenesLoaded = false;
        isLoadingScreen = false;

        currentSceneTransition.onStartTransitionCompleted -= OnStartTransitionCompleted;
        currentSceneTransition.onEndTransitionCompleted -= OnEndTransitionCompleted;


    }
    private void SetManager(List<SceneInfoSO> ScenesToLoad)
    {
        if (currentSceneTransition == null)
        {
            currentSceneTransition = SceneTransitions.ElementAt(0);
        }

        currentSceneTransition.onStartTransitionCompleted += OnStartTransitionCompleted;
        currentSceneTransition.onEndTransitionCompleted += OnEndTransitionCompleted;

        _ScenesToLoad = ScenesToLoad;
        SceneLoader.SetPersistentScenes(PersistentScenes);
        int sceneCount = _ScenesToLoad.Count;
 
        SceneInfoSO scene;
        for (int i = 0; i < sceneCount; i++)
        {
            scene = _ScenesToLoad.ElementAt(i);
            if (scene.needsInitialisation)
            {
                SceneInitedIndexesDict.Add(scene.buildIndex, false);
            }
            CurrentLoadingScenes.Add(scene);
        }
        if (SceneInitedIndexesDict.Count == 0)
        {
            _allScenesInited = true;
        }
    }
    private void StartLoadingScenes()
    {
        onSceneLoaded = SceneLoader.onSceneLoaded;
        SceneLoader.LoadScenes(_ScenesToLoad);

    }

    private void ShowLoadingScreen()
    {        
        //Scene persistentScene = SceneManager.GetSceneByBuildIndex(PersistentScenes[0].buildIndex);
        Scene persistentScene = gameObject.scene;
        currentSceneTransition.StartTransition(this, persistentScene);
    }
    private void OnStartTransitionCompleted()
    {
        isLoadingScreen = true;
        StartLoadingScenes();
        StartCoroutine(SetSceneLoadingProgressBar());
    }
    private IEnumerator SetSceneLoadingProgressBar()
    {
        loadingProgress = SceneLoader.GetLoadingProgress();
        currentSceneTransition.SetProgressValue(loadingProgress);

        while (loadingProgress != 1)
        {
            loadingProgress = SceneLoader.GetLoadingProgress();
            currentSceneTransition.SetProgressValue(loadingProgress);
            yield return null;
        }
    }
    //sets "sceneInfo.needsInitialisation" to true
    public void SetSceneInited(int sceneIndex)
    {
        bool isOk;
        if (SceneInitedIndexesDict.TryGetValue(sceneIndex, out isOk))
        {
            _initedSceneCount++;

        }
        if (SceneInitedIndexesDict.Count == _initedSceneCount)
        {
            _allScenesInited = true;
            onAllScenesInited?.Invoke();
            if (_allScenesLoaded)
            {
                onAllScenesOk?.Invoke();
            }
        }
    }
    private void OnScenesLoaded()
    {
        _allScenesLoaded = true;
        onAllScenesLoaded?.Invoke();
        if (_allScenesInited)
        {
            onAllScenesOk.Invoke();
        }
    }

    private void OnAllScenesOk()
    {
        onEndTransitionCompleted = currentSceneTransition.onEndTransitionCompleted;
        currentSceneTransition.EndTransition();
    }
    private void OnEndTransitionCompleted()
    {
        ResetManager();
    }
}
