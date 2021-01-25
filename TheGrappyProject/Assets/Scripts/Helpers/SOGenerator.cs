#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

public static class SOGenerator<T> where T : ScriptableObject
{
    public static UnityAction onAssetsRecompileCompleted;

    static void onDidReloadScripts()
    {
        onAssetsRecompileCompleted?.Invoke();
        Debug.Log("OnRecompileCompleted");
    }
    //Returns ScriptableObject reference. Creates in SciptableObject asset in AassetDatabase if not exists
    public static T GenerateSO(string name, string filepath)
    {
        if (!Directory.Exists(filepath))
        {
            Debug.LogError("Directory " + filepath + " does not exist");
            return null;
        }
        StringBuilder stringBuilder = new StringBuilder(50);
        stringBuilder.AppendFormat("{0}/{1}.asset", filepath, name);
        string assetPath = stringBuilder.ToString();
        T asset;
        if (!File.Exists(assetPath))
        {
            asset = CreateAsset(assetPath);
        }
        else
        {
            asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
        }
        AssetDatabase.SaveAssets();
        return asset;
    }

    //Generate Assets in folder.
    public static List<T> GenerateSOList(IEnumerable<string> Names, string filepath)
    {
        if (!Directory.Exists(filepath))
        {
            Debug.LogError("Directory " + filepath + " does not exist");
            return null;
        }
        
        List<T> SriptableObjects = new List<T>(Names.Count());
        StringBuilder stringBuilder = new StringBuilder(50);
        string assetPath;
        foreach(var name in Names)
        {
            stringBuilder.Clear();
            stringBuilder.AppendFormat("{0}/{1}.asset", filepath, name);
            assetPath = stringBuilder.ToString();
            if (!File.Exists(assetPath))
            {
                SriptableObjects.Add(CreateAsset(assetPath));
            }
            else
            {
                Debug.LogWarningFormat("File {0} already exists", assetPath);
            }
        }
        AssetDatabase.SaveAssets();
        return SriptableObjects;
    }
    static T CreateAsset(string path)
    {
        T asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, path);
        return asset;
    }
}
#endif