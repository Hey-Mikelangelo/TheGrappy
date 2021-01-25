using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoaderSO", menuName = "Scene Data/SceneLoader")]
public class SceneLoaderSO : ScriptableObject
{
    public SceneInfoHolderSO sceneInfoHolderSO;
    public SceneInfoSO alwaysLoadScene;
    public UnityAction onScenesLoaded;

    private List<int> PersistentScenesIndexes = new List<int>();
    private List<int> ScenesToUnloadIndexes = new List<int>();
    private List<int> ScenesToLoadIndexes = new List<int>();


    private int _newActiveSceneIndex;
    public void Quit()
    {
        Application.Quit();
    }
    /*private void OnEnable()
    {
        AsyncLoader.onSceneLoaded += OnSceneLoaded;
        AsyncLoader.onSceneUnloaded += OnSceneUnloaded;
    }
    private void OnDisable()
    {
        AsyncLoader.onSceneLoaded -= OnSceneLoaded;
        AsyncLoader.onSceneUnloaded -= OnSceneUnloaded;
    }*/
    /// <summary>
    /// Unloads all active scenes exluding persistent scenes,
    /// loads "Scenes" and sets first scene in the list as active scene
    /// </summary>
    /// <param name="Scenes"></param>
    public void LoadScenes(IEnumerable<SceneInfoSO> Scenes)
    {
        _newActiveSceneIndex = Scenes.ElementAt(0).buildIndex;
        ScenesToLoadIndexes = GetScenesToLoadIndexes(Scenes);
        ScenesToUnloadIndexes = GetScenesToUnloadIndexes();
        UnloadScenes(ScenesToUnloadIndexes);
        //Continues in "OnSceneUnloaded"

    }
    public float GetLoadingProgress()
    {
        float progress = 0;
        progress += AsyncLoader.GetScenesProgress();
        progress += AsyncLoader.GetUnloadingProgress();
        progress /= 2;
        return progress;
    }
    private void OnSceneLoaded(int buildIndex)
    {
        if (AsyncLoader.ScenesLeftToLoad <= 0)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_newActiveSceneIndex));
            Debug.Log("active scene: "+ SceneManager.GetActiveScene().name);
            onScenesLoaded?.Invoke();
        }
    }
    private void OnSceneUnloaded(int buildIndex)
    {
        if (AsyncLoader.ScenesLeftToUnload <= 0 )
        {
            LoadScenesAdditive(ScenesToLoadIndexes);
        }
    }
    public void AddPersistentScene(SceneInfoSO sceneInfo)
    {
        if (!PersistentScenesIndexes.Contains(sceneInfo.buildIndex))
        {
            PersistentScenesIndexes.Add(sceneInfo.buildIndex);
        }
    }
    public void RemovePersistentScene(SceneInfoSO sceneInfo)
    {
        if (!PersistentScenesIndexes.Contains(sceneInfo.buildIndex))
        {
            PersistentScenesIndexes.Remove(sceneInfo.buildIndex);
        }
    }
    public void SetPersistentScenes(IEnumerable<SceneInfoSO> SceneInfos)
    {
        PersistentScenesIndexes.Clear();
        foreach (var sceneInfo in SceneInfos)
        {
            PersistentScenesIndexes.Add(sceneInfo.buildIndex);
        }
    }
    private List<int> GetScenesToUnloadIndexes()
    {
        int sceneCount = SceneManager.sceneCount;
        List<int> Scenes = new List<int>(sceneCount);
        int index;
        for (int i = 0; i < sceneCount; i++)
        {
            index = SceneManager.GetSceneAt(i).buildIndex;
            if (!PersistentScenesIndexes.Contains(index) && alwaysLoadScene.buildIndex != index)
            {
                Scenes.Add(index);

            }
        }
        return Scenes;
    }
    private List<int> GetScenesToLoadIndexes(IEnumerable<SceneInfoSO> SceneInfos)
    {
        int sceneCount = SceneInfos.Count();
        List<int> Scenes = new List<int>(sceneCount);
        foreach(var info in SceneInfos)
        {
            Scenes.Add(info.buildIndex);
        }
        
        return Scenes;
    }
    public void LoadAlwaysLoadScene()
    {
        if (!IsLoadedAlwaysLoadScene())
        {
            SceneManager.LoadScene(alwaysLoadScene.buildIndex, LoadSceneMode.Additive);
        }
    }
    bool IsLoadedAlwaysLoadScene()
    {
        if(alwaysLoadScene == null)
        {
            return true;
        }
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex == alwaysLoadScene.buildIndex)
            {
                return true;
            }
        }
        return false;
    }
    private void UnloadScenes(IEnumerable<int> SceneIndexes)
    {
        if(SceneIndexes.Count() == 0)
        {
            LoadScenesAdditive(ScenesToLoadIndexes);
            return;
        }
        foreach(int buildIndex in SceneIndexes)
        {
            AsyncLoader.UnloadSceneAsync(buildIndex);
        }
        //Continues in "OnSceneUnloaded"
    }
    private void LoadScenesAdditive(IEnumerable<int> SceneIndexes)
    {
        foreach(int buildIndex in SceneIndexes)
        {
            AsyncOperation asyncOp = AsyncLoader.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        }
        //Continues in "OnSceneLoaded"
    }
}
