using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static UnityAction onScenesLoaded;
    public static UnityAction<int> onSceneLoaded;
    public static UnityAction<int> onSceneUnloaded;
    private static List<int> PersistentScenesIndexes = new List<int>();
    private static List<int> ScenesToUnloadIndexes = new List<int>();
    private static List<int> ScenesToLoadIndexes = new List<int>();


    private static int _newActiveSceneIndex;
    private static void OnEnable()
    {
        AsyncLoader.onSceneLoaded += OnSceneLoaded;
        AsyncLoader.onSceneUnloaded += OnSceneUnloaded;
    }
    private static void OnDisable()
    {
        AsyncLoader.onSceneLoaded -= OnSceneLoaded;
        AsyncLoader.onSceneUnloaded -= OnSceneUnloaded;
    }
    /// <summary>
    /// Unloads all active scenes exluding persistent scenes,
    /// loads "Scenes" and sets first scene in the list as active scene
    /// </summary>
    /// <param name="Scenes"></param>
    public static void LoadScenes(IEnumerable<SceneInfoSO> Scenes)
    {
        if(Scenes.Count() == 0)
        {
            onScenesLoaded?.Invoke();
            return;
        }
        onSceneLoaded = AsyncLoader.onSceneLoaded;
        onSceneUnloaded = AsyncLoader.onSceneUnloaded;

        AsyncLoader.onSceneLoaded += OnSceneLoaded;
        AsyncLoader.onSceneUnloaded += OnSceneUnloaded;

        _newActiveSceneIndex = Scenes.ElementAt(0).buildIndex;
        ScenesToLoadIndexes = GetScenesToLoadIndexes(Scenes);
        ScenesToUnloadIndexes = GetScenesToUnloadIndexes();
        UnloadScenes(ScenesToUnloadIndexes);
        //Continues in "OnSceneUnloaded"

    }
    public static float GetLoadingProgress()
    {
        float progress = 0;
        progress += AsyncLoader.GetScenesProgress();
        progress += AsyncLoader.GetUnloadingProgress();
        progress /= 2;
        return progress;
    }
    private static void OnSceneLoaded(int buildIndex)
    {
        if (AsyncLoader.ScenesLeftToLoad <= 0)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_newActiveSceneIndex));
            onScenesLoaded?.Invoke();
            AsyncLoader.onSceneLoaded -= OnSceneLoaded;
            AsyncLoader.onSceneUnloaded -= OnSceneUnloaded;
        }
    }
    private static void OnSceneUnloaded(int buildIndex)
    {
        if (AsyncLoader.ScenesLeftToUnload <= 0)
        {
            LoadScenesAdditive(ScenesToLoadIndexes);
        }
    }
    public static void AddPersistentScene(SceneInfoSO sceneInfo)
    {
        if (!PersistentScenesIndexes.Contains(sceneInfo.buildIndex))
        {
            PersistentScenesIndexes.Add(sceneInfo.buildIndex);
        }
    }
    public static void RemovePersistentScene(SceneInfoSO sceneInfo)
    {
        if (PersistentScenesIndexes.Contains(sceneInfo.buildIndex))
        {
            PersistentScenesIndexes.Remove(sceneInfo.buildIndex);
        }
    }
    public static void SetPersistentScenes(IEnumerable<SceneInfoSO> SceneInfos)
    {
        PersistentScenesIndexes.Clear();
        foreach (var sceneInfo in SceneInfos)
        {
            PersistentScenesIndexes.Add(sceneInfo.buildIndex);
        }
    }
    private static List<int> GetScenesToUnloadIndexes()
    {
        int sceneCount = SceneManager.sceneCount;
        List<int> Scenes = new List<int>(sceneCount);
        int index;
        for (int i = 0; i < sceneCount; i++)
        {
            index = SceneManager.GetSceneAt(i).buildIndex;
            if (!PersistentScenesIndexes.Contains(index))
            {
                Scenes.Add(index);

            }
        }
        return Scenes;
    }
    private static List<int> GetScenesToLoadIndexes(IEnumerable<SceneInfoSO> SceneInfos)
    {
        int sceneCount = SceneInfos.Count();
        List<int> Scenes = new List<int>(sceneCount);
        foreach (var info in SceneInfos)
        {
            Scenes.Add(info.buildIndex);
        }

        return Scenes;
    }
   
    private static void UnloadScenes(IEnumerable<int> SceneIndexes)
    {
        if (SceneIndexes.Count() == 0)
        {
            LoadScenesAdditive(ScenesToLoadIndexes);
            return;
        }
        foreach (int buildIndex in SceneIndexes)
        {
            AsyncLoader.UnloadSceneAsync(buildIndex);
        }
        //Continues in "OnSceneUnloaded"
    }
    private static void LoadScenesAdditive(IEnumerable<int> SceneIndexes)
    {
        foreach (int buildIndex in SceneIndexes)
        {
            AsyncOperation asyncOp = AsyncLoader.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        }
        //Continues in "OnSceneLoaded"
    }
}
