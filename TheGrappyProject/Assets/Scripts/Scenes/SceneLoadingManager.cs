using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoadingManager : MonoBehaviour
{
    [Inject] private SceneLoadingChannelSO sceneLoadingChannelSO;

    public System.Action<int> onSceneLoaded;
    
    [SerializeField] private List<SceneInfoSO> persistentScenes = new List<SceneInfoSO>();
    [SerializeField] private List<SceneTransitionSO> sceneTransitions = new List<SceneTransitionSO>();
    [SerializeField] private List<SceneInfoSO> scenesToLoadOnStart = new List<SceneInfoSO>();

    //Loading scenes
    private List<SceneInfoSO> currentLoadingScenes = new List<SceneInfoSO>();
    private Dictionary<int, bool> sceneInitedIndexesDict = new Dictionary<int, bool>();

    //Loading screen
    public bool isLoadingScreen;
    public float loadingProgress;

    //Events
    public System.Action onAllScenesInited;
    public System.Action onAllScenesLoaded;
    public System.Action onAllScenesOk;
    public System.Action onEndTransitionCompleted;

    //Scene transitions
    private SceneTransitionSO currentSceneTransition;

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
        SceneLoader.OnScenesLoaded += OnScenesLoaded;

    }
    public void OnDestroy()
    {
        sceneLoadingChannelSO.onLoadScenes -= LoadScenes;
        sceneLoadingChannelSO.onSceneInted -= SetSceneInited;
        sceneLoadingChannelSO.onSetSceneTransition -= SetSceneTransition;
        sceneLoadingChannelSO.onQuit -= Quit;
        onAllScenesOk -= OnAllScenesOk;
        SceneLoader.OnScenesLoaded -= OnScenesLoaded;
    }
    private void Awake()
    {
        sceneLoadingChannelSO.SetInitSceneLoaded();
    }
    private void Start()
    {
        SetSceneTransition(0);
        LoadScenes(scenesToLoadOnStart, true);
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
            if (sceneTransitions.Count < 1)
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
        foreach (var scene in persistentScenes)
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
        if (indexInSceneTranstionList >= 0 && indexInSceneTranstionList < sceneTransitions.Count)
        {
            currentSceneTransition = sceneTransitions.ElementAt(indexInSceneTranstionList);
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
        sceneInitedIndexesDict.Clear();
        currentLoadingScenes.Clear();
        _initedSceneCount = 0;
        _allScenesInited = false;
        _allScenesLoaded = false;
        isLoadingScreen = false;

       
    }
    private void SetManager(List<SceneInfoSO> ScenesToLoad)
    {
        if (currentSceneTransition == null)
        {
            currentSceneTransition = sceneTransitions.ElementAt(0);
        }

        currentSceneTransition.onStartTransitionCompleted += OnStartTransitionCompleted;
        currentSceneTransition.onEndTransitionCompleted += OnEndTransitionCompleted;

        _ScenesToLoad = ScenesToLoad;
        SceneLoader.SetPersistentScenes(persistentScenes);
        int sceneCount = _ScenesToLoad.Count;
 
        SceneInfoSO scene;
        for (int i = 0; i < sceneCount; i++)
        {
            scene = _ScenesToLoad.ElementAt(i);
            if (scene.needsInitialisation)
            {
                sceneInitedIndexesDict.Add(scene.buildIndex, false);
            }
            currentLoadingScenes.Add(scene);
        }
        if (sceneInitedIndexesDict.Count == 0)
        {
            _allScenesInited = true;
        }
    }
    private void StartLoadingScenes()
    {
        onSceneLoaded = SceneLoader.OnSceneLoaded;
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
        if (sceneInitedIndexesDict.TryGetValue(sceneIndex, out isOk))
        {
            _initedSceneCount++;
        }
        Debug.Log(sceneInitedIndexesDict.Count);
        Debug.Log(_initedSceneCount);

        if (sceneInitedIndexesDict.Count == _initedSceneCount)
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
        sceneLoadingChannelSO.onScenesAllOk?.Invoke();
        currentSceneTransition.EndTransition();
    }
    private void OnEndTransitionCompleted()
    {
        ResetManager();
        currentSceneTransition.onStartTransitionCompleted -= OnStartTransitionCompleted;
        currentSceneTransition.onEndTransitionCompleted -= OnEndTransitionCompleted;


    }
}
