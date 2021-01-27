#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    private void OnEnable()
    {
        EditorBuildSettings.sceneListChanged += UpdateAssets;
    }
    private void OnDisable()
    {
        EditorBuildSettings.sceneListChanged -= UpdateAssets;
    }
    private void OnDestroy()
    {

    }
    public void UpdateAssets()
    {
        Debug.Log("Update assets");
        assetsFolderPath = sceneInfoHolderSO.sceneInfoSOPath;
        InspectForNullReferences();
        SetSceneInfos();
        DeleteAndUpdateAssets();
        CreateAssets();
    }
    int TryGetUpdatedSceneInBuildSettings(sceneInfo sceneInfo)
    {
        //for each scene in build settings check if scene actualy is removed or just changed name, path or build index
        for (int i = 0; i < BuildSceneInfos.Count; i++)
        {
            sceneInfo scene = BuildSceneInfos.ElementAt(i);
            //check name and index
            if (scene.name == sceneInfo.name && scene.buildIndex == sceneInfo.buildIndex)
            {
                return i;
            }
            //check name and path
            else if (scene.name == sceneInfo.name && scene.path == sceneInfo.path)
            {
                return i;
            }
            //check index
            else if (scene.buildIndex == sceneInfo.buildIndex)
            {
                return i;
            }
        }
        return -1;
    }
    void DeleteAndUpdateAssets()
    {
        int count = sceneInfoHolderSO.SceneInfoList.Count;
        List<SceneInfoSO> SceneInfoListCopy = new List<SceneInfoSO>(sceneInfoHolderSO.SceneInfoList);
        for (int i = 0; i < count; i++)
        {
            SceneInfoSO sceneInfoSO = SceneInfoListCopy.ElementAt(i);
            sceneInfo scene = (sceneInfo)sceneInfoSO;
            //because from build settings we cannot get info does scene needsInitialisation 
            scene.needsInitialisation = false;
            if (!BuildSceneInfos.Contains(scene))
            {
                string oldName = scene.name;
                int maybeIndex = TryGetUpdatedSceneInBuildSettings(scene);
                if (maybeIndex != -1)
                {
                    sceneInfo buildSettingsSceneInfo = BuildSceneInfos.ElementAt(i);
                    sceneInfoSO.name = buildSettingsSceneInfo.name;
                    sceneInfoSO.path = buildSettingsSceneInfo.path;
                    sceneInfoSO.buildIndex = buildSettingsSceneInfo.buildIndex;
                    string assetPath = assetsFolderPath + "/" + oldName + ".asset";
                    Debug.Log(assetPath);
                    AssetDatabase.RenameAsset(assetPath, buildSettingsSceneInfo.name);
                }
                else
                {
                    string pathToAsset = assetsFolderPath + "/" + scene.name + ".asset";
                    Debug.Log("delete: " + pathToAsset + " - index: " + i);
                    sceneInfoHolderSO.SceneInfoList.RemoveAt(i);
                    AssetDatabase.MoveAssetToTrash(pathToAsset);
                }
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    void InspectForNullReferences()
    {
        for (int i = 0; i < sceneInfoHolderSO.SceneInfoList.Count; i++)
        {
            if (sceneInfoHolderSO.SceneInfoList.ElementAt(i) == null)
            {
                sceneInfoHolderSO.SceneInfoList.RemoveAt(i);
            }
        }
    }
    void SetSceneInfos()
    {
        BuildSceneInfos = GetBuildSettingsScenes();
    }

    private void CreateAssets()
    {
        List<string> SceneNames = new List<string>(sceneCount);
        List<sceneInfo> SceneInfoList = new List<sceneInfo>(sceneCount);
        foreach (var elem in sceneInfoHolderSO.SceneInfoList)
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
                EditorUtility.SetDirty(newSceneInfoSO);
            }

        }
        EditorUtility.SetDirty(sceneInfoHolderSO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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
                (
                    name: Path.GetFileNameWithoutExtension(path),
                    path: Path.GetFullPath(path),
                    buildIndex: SceneUtility.GetBuildIndexByScenePath(path)
                );
                Scenes.Add(sceneInfo);
                validSceneCount++;
            }
        }
        sceneCount = validSceneCount;
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return Scenes;
    }
}
#endif