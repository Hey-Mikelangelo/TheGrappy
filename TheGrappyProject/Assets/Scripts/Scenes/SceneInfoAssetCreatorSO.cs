#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneInfoAssetCreatorSO", menuName = "Scene Data/SceneInfoAssetCreator")]
public class SceneInfoAssetCreatorSO : ScriptableObject
{    
    public SceneInfoHolderSO sceneInfoHolderSO;
    public int sceneCount;
    private string assetsFolderPath;
    List<sceneInfo> BuildSceneInfos = new List<sceneInfo>();
    public bool refreshed = false;

    private void OnEnable()
    {
        if (!refreshed)
        {
            UpdateAssets();
            refreshed = true;
        }
        EditorApplication.quitting += OnQuittingEditor;
        EditorBuildSettings.sceneListChanged += UpdateAssets;
    }
    void OnQuittingEditor()
    {
        refreshed = false;
    }
    private void OnDisable()
    {
        EditorBuildSettings.sceneListChanged -= UpdateAssets;
    }
    private void OnDestroy()
    {

    }
    void DeleteRemovedSceneAssets()
    {
        int count = sceneInfoHolderSO.SceneInfoList.Count;
        //for each item in existing scene list find corresponding scene in build settings
        for (int i = 0; i < count; i++)
        {
            SceneInfoSO item = sceneInfoHolderSO.SceneInfoList.ElementAt(i);
            //if corresponding scene does not exists in buildSettings
            if (!BuildSceneInfos.Contains((sceneInfo)item)){
                //delete sceneInfoSO Asset
                string pathToAsset = assetsFolderPath + "/" + item.name + ".asset";
                AssetDatabase.DeleteAsset(pathToAsset);
                AssetDatabase.SaveAssets();
                //remove from list
                sceneInfoHolderSO.SceneInfoList.RemoveAt(i);
            }
        }
    }
    void InspectForNullReferences()
    {
        for(int i = 0; i < sceneInfoHolderSO.SceneInfoList.Count; i++)
        {
            if(sceneInfoHolderSO.SceneInfoList.ElementAt(i) == null)
            {
                sceneInfoHolderSO.SceneInfoList.RemoveAt(i);
            }
        }
    }
    void SetSceneInfos()
    {
        BuildSceneInfos = GetBuildSettingsScenes();
    }
    public void UpdateAssets()
    {
        Debug.Log("Update assets");
        assetsFolderPath = sceneInfoHolderSO.sceneInfoSOPath;
        InspectForNullReferences();
        SetSceneInfos();
        DeleteRemovedSceneAssets();
        CreateAssets();
    }
    private void CreateAssets()
    {

        List<string> SceneNames = new List<string>(sceneCount);
        List<sceneInfo> SceneInfoList = new List<sceneInfo>(sceneCount);
        foreach(var elem in sceneInfoHolderSO.SceneInfoList)
        {
            SceneInfoList.Add((sceneInfo)elem);
        }
        if (!Directory.Exists(assetsFolderPath))
        {
            Directory.CreateDirectory(assetsFolderPath);
        }
        //search every scene in build setting and if corresponding SceneInfoSO asset is not created - create one
        foreach (var buildSceneInfo in BuildSceneInfos)
        {
            if (!SceneInfoList.Contains(buildSceneInfo))
            {
                SceneInfoSO newSceneInfoSO = SOGenerator<SceneInfoSO>.GenerateSO(buildSceneInfo.name, assetsFolderPath);
                newSceneInfoSO.setValues(
                    buildSceneInfo.name,
                    buildSceneInfo.path,
                    buildSceneInfo.buildIndex
                    );
                sceneInfoHolderSO.SceneInfoList.Add(newSceneInfoSO);
            }
                        
        }
    }
    public List<sceneInfo> GetBuildSettingsScenes()
    {
        EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
        int count = buildScenes.Length;
        int validSceneCount = 0;
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
                    path = Path.GetFullPath(path),
                    buildIndex = SceneUtility.GetBuildIndexByScenePath(path)
                };
                Scenes.Add(sceneInfo);
                validSceneCount++;
            }
        }
        sceneCount = validSceneCount;
        return Scenes;
    }
}
#endif