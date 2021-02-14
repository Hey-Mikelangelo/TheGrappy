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
    public List<SceneInfoSO> ScenesToLoadOnStart = new List<SceneInfoSO>();

    //Loading scenes
    public UnityAction<int> onSceneLoaded;
    private List<SceneInfoSO> _CurrentLoadingScenes = new List<SceneInfoSO>();
    private Dictionary<int, bool> _SceneInitedIndexesDict = new Dictionary<int, bool>();

    //Loading screen
    public bool isLoadingScreen;
    public float loadingProgress;

    //Events
    public UnityAction onAllScenesInited;
    public UnityAction onAllScenesLoaded;
    public UnityAction onAllScenesOk;
    public UnityAction onEndTransitionCompleted;

    //Scene transitions
    private SceneTransitionSO _currentSceneTransition;

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
    private void Awake()
    {
        sceneLoadingChannelSO.SetInitSceneLoaded();
    }
    private void Start()
    {
        SetSceneTransition(0);
        LoadScenes(ScenesToLoadOnStart, true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadScenes(List<SceneInfoSO> ScenesToLoad, bool showLoadingScreen)
    {
        ResetManager();
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
        _currentSceneTransition = transitionSO;
    }
    public bool SetSceneTransition(int indexInSceneTranstionList)
    {
        if (indexInSceneTranstionList >= 0 && indexInSceneTranstionList < SceneTransitions.Count)
        {
            _currentSceneTransition = SceneTransitions.ElementAt(indexInSceneTranstionList);
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
        _SceneInitedIndexesDict.Clear();
        _CurrentLoadingScenes.Clear();
        _initedSceneCount = 0;
        _allScenesInited = false;
        _allScenesLoaded = false;
        isLoadingScreen = false;

       
    }
    private void SetManager(List<SceneInfoSO> ScenesToLoad)
    {
        if (_currentSceneTransition == null)
        {
            _currentSceneTransition = SceneTransitions.ElementAt(0);
        }

        _currentSceneTransition.onStartTransitionCompleted += OnStartTransitionCompleted;
        _currentSceneTransition.onEndTransitionCompleted += OnEndTransitionCompleted;

        _ScenesToLoad = ScenesToLoad;
        SceneLoader.SetPersistentScenes(PersistentScenes);
        int sceneCount = _ScenesToLoad.Count;
 
        SceneInfoSO scene;
        for (int i = 0; i < sceneCount; i++)
        {
            scene = _ScenesToLoad.ElementAt(i);
            if (scene.needsInitialisation)
            {
                _SceneInitedIndexesDict.Add(scene.buildIndex, false);
            }
            _CurrentLoadingScenes.Add(scene);
        }
        if (_SceneInitedIndexesDict.Count == 0)
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
        _currentSceneTransition.StartTransition(this, persistentScene);
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
        _currentSceneTransition.SetProgressValue(loadingProgress);

        while (loadingProgress != 1)
        {
            loadingProgress = SceneLoader.GetLoadingProgress();
            _currentSceneTransition.SetProgressValue(loadingProgress);
            yield return null;
        }
    }
    //sets "sceneInfo.needsInitialisation" to true
    public void SetSceneInited(int sceneIndex)
    {
        bool isOk;
        if (_SceneInitedIndexesDict.TryGetValue(sceneIndex, out isOk))
        {
            _initedSceneCount++;

        }
        if (_SceneInitedIndexesDict.Count == _initedSceneCount)
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
        onEndTransitionCompleted = _currentSceneTransition.onEndTransitionCompleted;
        sceneLoadingChannelSO.onScenesAllOk?.Invoke();
        _currentSceneTransition.EndTransition();
    }
    private void OnEndTransitionCompleted()
    {
        ResetManager();
        _currentSceneTransition.onStartTransitionCompleted -= OnStartTransitionCompleted;
        _currentSceneTransition.onEndTransitionCompleted -= OnEndTransitionCompleted;


    }
}
