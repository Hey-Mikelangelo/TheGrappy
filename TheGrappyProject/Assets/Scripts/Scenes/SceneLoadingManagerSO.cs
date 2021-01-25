using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoadingManagerSO", menuName = "Scene Data/Scene loading manager")]
public class SceneLoadingManagerSO : ScriptableObject
{
    public List<SceneInfoSO> PersistentScenes = new List<SceneInfoSO>();
    public List<SceneTransitionSO> SceneTransitions = new List<SceneTransitionSO>();
    //Loading scenes
    [HideInInspector] public List<SceneInfoSO> CurrentLoadingScenes = new List<SceneInfoSO>();
    [HideInInspector] public Dictionary<sceneInfo, bool> SceneInitedDict = new Dictionary<sceneInfo, bool>();
    [HideInInspector] public UnityAction<int> onSceneLoaded;

    //Loading screen
    [HideInInspector] public bool isLoadingScreen;
    [HideInInspector] public float loadingProgress;

    //Events
    [HideInInspector] public UnityAction onAllScenesInited;
    [HideInInspector] public UnityAction onAllScenesLoaded;
    [HideInInspector] public UnityAction onAllScenesOk;
    public UnityAction onEndTransitionCompleted;

    //Scene transitions
    public SceneTransitionSO currentSceneTransition;

    private int _initedSceneCount;
    private MonoBehaviour _persistentCoroutineCaller;
    private List<SceneInfoSO> _ScenesToLoad;
    private bool _allScenesInited, _allScenesLoaded;

    public void OnEnable()
    {
        Debug.Log("OnEnable Manager");
    }
    public void OnDestroy()
    {
        Debug.Log("OnDestroy Manager");
    }
    public void SetPersistentCourotineCaller(MonoBehaviour coroutineCaller)
    {
        _persistentCoroutineCaller = coroutineCaller;

    }
    public void LoadScenes(List<SceneInfoSO> ScenesToLoad, bool showLoadingScreen)
    {
        ResetAndSetScenesToLoad(ScenesToLoad);
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
    public void LoadPersistentScenes()
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
    private void ResetAndSetScenesToLoad(List<SceneInfoSO> ScenesToLoad)
    {
        _ScenesToLoad = ScenesToLoad;
        SceneInitedDict.Clear();
        CurrentLoadingScenes.Clear();
        //SceneLoader.SetPersistentScenes(PersistentScenes);
        _initedSceneCount = 0;
        _allScenesInited = false;
        _allScenesLoaded = false;
        int sceneCount = _ScenesToLoad.Count;
        onAllScenesOk += OnAllScenesOk;
        SceneInfoSO scene;
        for (int i = 0; i < sceneCount; i++)
        {
            scene = _ScenesToLoad.ElementAt(i);
            if (scene.needsInitialisation)
            {
                SceneInitedDict.Add((sceneInfo)scene, false);
            }
            CurrentLoadingScenes.Add(scene);
        }
        if(SceneInitedDict.Count == 0)
        {
            _allScenesInited = true;
        }
    }
    private void StartLoadingScenes()
    {
        onSceneLoaded = SceneLoader.onSceneLoaded;
        SceneLoader.onScenesLoaded += OnScenesLoaded;
        SceneLoader.LoadScenes(_ScenesToLoad);

    }

    private void ShowLoadingScreen()
    {
        Debug.Log("Show Loading screen");
        if(currentSceneTransition == null) {
            currentSceneTransition = SceneTransitions.ElementAt(0);
        }
        currentSceneTransition.onStartTransitionCompleted += OnStartTransitionCompleted;
        Scene persistentScene = SceneManager.GetSceneByBuildIndex(PersistentScenes[0].buildIndex);
        currentSceneTransition.StartTransition(_persistentCoroutineCaller, persistentScene);
    }
    private void OnStartTransitionCompleted()
    {
        Debug.Log("OnStartTransitionCompleted");
        isLoadingScreen = true;
        StartLoadingScenes();
        _persistentCoroutineCaller.StartCoroutine(SetSceneLoadingProgressBar());
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
    public void SetSceneInited(sceneInfo scene)
    {
        bool isOk;
        scene.needsInitialisation = true;
        if(SceneInitedDict.TryGetValue(scene, out isOk))
        {

        }
        
        _initedSceneCount++;
        if (SceneInitedDict.Count == _initedSceneCount)
        {
            _allScenesInited = true;
            onAllScenesInited?.Invoke();
            if (_allScenesLoaded)
            {
                onAllScenesOk?.Invoke();
                Debug.Log("onAllScenesOk inited");

            }
        }
    }
    private void OnScenesLoaded()
    {
        _allScenesLoaded = true;
        onAllScenesLoaded?.Invoke();
        Debug.Log("OnScenesLoaded");
        if (_allScenesInited)
        {
            onAllScenesOk.Invoke();
            Debug.Log("onAllScenesOk loaded");
        }
    }

    private void OnAllScenesOk()
    {
        Debug.Log("OnAllScenesOk");
        onEndTransitionCompleted = currentSceneTransition.onEndTransitionCompleted;
        onEndTransitionCompleted += OnEndTransitionCompleted;
        currentSceneTransition.EndTransition();
    }
    private void OnEndTransitionCompleted()
    {
        Debug.Log("OnEndTransitionCompleted");
        isLoadingScreen = false;
    }
}
