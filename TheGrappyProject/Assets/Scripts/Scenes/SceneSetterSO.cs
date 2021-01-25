using UnityEngine;

//[CreateAssetMenu(fileName = "SceneSetterSO", menuName = "Scene Data/Scene Setter")]
public class SceneSetterSO : ScriptableObject
{
    /*public SceneInfoHolderSO sceneInfoHolderSO;
    private List<sceneInfo> BuildScenes;
    private bool _isWaitingForRefresh = false;
    public int sceneCount { get; private set; }
    private void OnEnable()
    {
        EditorBuildSettings.sceneListChanged += SetScenes;
        EnumGenerator.onAssetsRecompileCompleted += OnSceneEnumGenerated;
    }
    private void OnDisable()
    {
        EditorBuildSettings.sceneListChanged -= SetScenes;
        EnumGenerator.onAssetsRecompileCompleted -= OnSceneEnumGenerated;

    }
    public void SetScenes()
    {
        BuildScenes = GetBuildSettingsScenes();
        GenerateSceneEnums(BuildScenes);
    }
   
    public void PrintDictVars()
    {
        foreach (var info in sceneInfoHolderSO.SceneDict.Values)
        {
            Debug.Log(info.path);
        }
    }
    public List<sceneInfo> GetBuildSettingsScenes()
    {
        EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
        int count = buildScenes.Length;
        List<sceneInfo> Scenes = new List<sceneInfo>(count);
        sceneInfo sceneInfo;
        for (int i = 0; i < count; i++)
        {
            string path = buildScenes[i].path;
            if (File.Exists(path))
            {
                sceneInfo = new sceneInfo
                {
                    name = Path.GetFileNameWithoutExtension(path),
                    path = path,
                    buildIndex = SceneUtility.GetBuildIndexByScenePath(path)
                };
                Scenes.Add(sceneInfo);
            }
        }
        sceneCount = Scenes.Count();
        return Scenes;
    }
    public IEnumerable<Scene> GetBuildSettingsScenesLazy()
    {
        EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
        Scene scene;
        for (int i = 0; i < buildScenes.Length; i++)
        {
            scene = SceneManager.GetSceneByPath(buildScenes[i].path);
            if (scene != null)
            {
                yield return scene;
            }
        }
    }
    private void GenerateSceneEnums(IEnumerable<sceneInfo> Scenes)
    {
        List<string> sceneNames = new List<string>(sceneCount);
        for (int i = 0; i < sceneCount; i++)
        {
            sceneNames.Add(Scenes.ElementAt(i).name);
        }
        _isWaitingForRefresh = true;
        EnumGenerator.GenerateEnum("SceneEnum", sceneNames.ToArray());

    }
    private void OnSceneEnumGenerated()
    {
        if (!_isWaitingForRefresh)
        {
            return;
        }
        _isWaitingForRefresh = false;
        EnumGenerator.onAssetsRecompileCompleted -= OnSceneEnumGenerated;
        var enums = Enum.GetValues(typeof(SceneEnum));
        int i = 0;
        sceneInfoHolderSO.SceneDict.Clear();
        //sceneInfoHolderSO.SceneInfoList.Clear();
        foreach (SceneEnum sceneEnum in enums)
        {
            sceneInfoHolderSO.SceneDict.Add(sceneEnum, BuildScenes.ElementAt(i));
            Debug.Log(sceneInfoHolderSO.SceneDict[sceneEnum].name);
            i++;
        }
        EditorUtility.SetDirty(sceneInfoHolderSO);
    }*/
    
}
