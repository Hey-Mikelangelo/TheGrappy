using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class AsyncLoader
{
    public static Dictionary<AsyncOperation, int> AsyncOpToSceneBuildIndexDict
        = new Dictionary<AsyncOperation, int>();

    public static UnityAction onNewSceneInitCompleted;

    public static UnityAction<int> onSceneLoaded;
    public static UnityAction<int> onSceneUnloaded;
    public static List<AsyncOperation> LoadScenesAsyncOperations = new List<AsyncOperation>();
    public static List<AsyncOperation> UnloadScenesAsyncOperations = new List<AsyncOperation>();
    public static List<AsyncOperation> OtherAsyncOperations = new List<AsyncOperation>();

    public static int ScenesLeftToUnload { get; private set; } = 0;
    public static int ScenesLeftToLoad { get; private set; } = 0;
    public static float ScenesProgress { get; private set; }
    public static float OtherProgress { get; private set; }
    private static void onAsyncLoadCompleted(AsyncOperation asyncOp)
    {
        int builIndex;
        //if operation was not a loadScene operation
        if (!AsyncOpToSceneBuildIndexDict.TryGetValue(asyncOp, out builIndex))
        {
            LoadScenesAsyncOperations.Remove(asyncOp);
            return;
        }
        else
        {
            AsyncOpToSceneBuildIndexDict.Remove(asyncOp);
            asyncOp.allowSceneActivation = true;
            LoadScenesAsyncOperations.Remove(asyncOp);
            ScenesLeftToLoad--;
            onSceneLoaded?.Invoke(builIndex);

        }
    }
    private static void onAsyncUnloadCompleted(AsyncOperation asyncOp)
    {
        int builIndex;
        //if operation was not a loadScene operation
        if (!AsyncOpToSceneBuildIndexDict.TryGetValue(asyncOp, out builIndex))
        {
            UnloadScenesAsyncOperations.Remove(asyncOp);
            return;
        }
        else
        {
            AsyncOpToSceneBuildIndexDict.Remove(asyncOp);
            UnloadScenesAsyncOperations.Remove(asyncOp);
            ScenesLeftToUnload--;
            onSceneUnloaded?.Invoke(builIndex);

        }
    }
    public static float GetScenesProgress()
    {
        if (ScenesLeftToLoad == 0)
        {
            return 1;
        }
        float progress = 0;
        int asyncOpCount = LoadScenesAsyncOperations.Count;
        for (int i = 0; i < asyncOpCount; i++)
        {
            progress += LoadScenesAsyncOperations[i].progress;
        }
        progress /= asyncOpCount;
        return progress;
    }
    public static float GetUnloadingProgress()
    {
        if (ScenesLeftToUnload == 0)
        {
            return 1;
        }
        float progress = 0;
        int asyncOpCount = UnloadScenesAsyncOperations.Count;
        for (int i = 0; i < asyncOpCount; i++)
        {
            progress += UnloadScenesAsyncOperations[i].progress;
        }
        progress /= asyncOpCount;
        return progress;
    }
    public static void RegisterLoadAsyncOperation(AsyncOperation asyncOperation)
    {
        LoadScenesAsyncOperations.Add(asyncOperation);
    }
    public static void AddUnoadAsyncOperation(AsyncOperation asyncOperation)
    {

    }
    /// <summary>
    /// Starts loading scene asynchronously. If register = true, adds it to the "LoadAsyncOperations" 
    /// list, deletes scene from this list when loaded and calls "onAsyncLoadCompleted" on scene load
    /// completion.
    /// </summary>
    /// <param name="buildIndex"></param>
    /// <param name="loadSceneMode"></param>
    /// <param name="register"></param>
    /// <returns></returns>
    public static AsyncOperation LoadSceneAsync(int buildIndex, LoadSceneMode loadSceneMode, bool register = true)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(buildIndex, loadSceneMode);
        if (register)
        {
            //add to dictionary to associate buildIndex with AsyncOperation
            AsyncOpToSceneBuildIndexDict.Add(asyncOp, buildIndex);
            //Subscribe to asyncOperation completion
            asyncOp.completed += onAsyncLoadCompleted;
            //Add to the list. Can be used from outside
            LoadScenesAsyncOperations.Add(asyncOp);
            ScenesLeftToLoad++;
        }

        return asyncOp;
    }
    /// <summary>
    /// Starts unloading scene asynchronously. If register = true, adds it to the "UnloadAsyncOperations" 
    /// list, deletes scene from this list when loaded and calls "onAsyncUnloadCompleted" on scene unload
    /// completion.
    /// </summary>
    /// <param name="buildIndex"></param>
    /// <param name="register"></param>
    /// <returns></returns>
    public static AsyncOperation UnloadSceneAsync(int buildIndex, bool register = true)
    {
        AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(buildIndex);
        if (register)
        {
            //add to dictionary to associate buildIndex with AsyncOperation
            AsyncOpToSceneBuildIndexDict.Add(asyncOp, buildIndex);
            //Subscribe to asyncOperation completion
            asyncOp.completed += onAsyncUnloadCompleted;
            //Add to the list. Can be used from outside
            UnloadScenesAsyncOperations.Add(asyncOp);
            ScenesLeftToUnload++;
        }

        return asyncOp;
    }
}
