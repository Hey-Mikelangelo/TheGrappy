using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static System.Action OnScenesLoaded;
    public static System.Action<int> OnSceneLoaded;
    public static System.Action<int> OnSceneUnloaded;

    private static List<int> persistentScenesIndexes = new List<int>();
    private static List<int> ScenesToUnloadIndexes = new List<int>();
    private static List<int> ScenesToLoadIndexes = new List<int>();


    private static int _newActiveSceneIndex;
    private static void OnEnable()
    {
        AsyncLoader.onSceneLoaded += SceneLoaded;
        AsyncLoader.onSceneUnloaded += SceneUnloaded;
    }
    private static void OnDisable()
    {
        AsyncLoader.onSceneLoaded -= SceneLoaded;
        AsyncLoader.onSceneUnloaded -= SceneUnloaded;
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
            OnScenesLoaded?.Invoke();
            return;
        }
        OnSceneLoaded = AsyncLoader.onSceneLoaded;
        OnSceneUnloaded = AsyncLoader.onSceneUnloaded;

        AsyncLoader.onSceneLoaded += SceneLoaded;
        AsyncLoader.onSceneUnloaded += SceneUnloaded;

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
    private static void SceneLoaded(int buildIndex)
    {
        if (AsyncLoader.ScenesLeftToLoad <= 0)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_newActiveSceneIndex));
            OnScenesLoaded?.Invoke();
            AsyncLoader.onSceneLoaded -= SceneLoaded;
            AsyncLoader.onSceneUnloaded -= SceneUnloaded;
        }
    }
    private static void SceneUnloaded(int buildIndex)
    {
        if (AsyncLoader.ScenesLeftToUnload <= 0)
        {
            LoadScenesAdditive(ScenesToLoadIndexes);
        }
    }
    public static void AddPersistentScene(SceneInfoSO sceneInfo)
    {
        if (!persistentScenesIndexes.Contains(sceneInfo.buildIndex))
        {
            persistentScenesIndexes.Add(sceneInfo.buildIndex);
        }
    }
    public static void RemovePersistentScene(SceneInfoSO sceneInfo)
    {
        if (persistentScenesIndexes.Contains(sceneInfo.buildIndex))
        {
            persistentScenesIndexes.Remove(sceneInfo.buildIndex);
        }
    }
    public static void SetPersistentScenes(IEnumerable<SceneInfoSO> SceneInfos)
    {
        persistentScenesIndexes.Clear();
        foreach (var sceneInfo in SceneInfos)
        {
            persistentScenesIndexes.Add(sceneInfo.buildIndex);
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
            if (!persistentScenesIndexes.Contains(index))
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
