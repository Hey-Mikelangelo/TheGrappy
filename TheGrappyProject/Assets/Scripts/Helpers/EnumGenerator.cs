#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEditor.Experimental;

public static class EnumGenerator {
    //if called without filepath, enum class will be created in EnumGenerator.enumsDirectory
    public static string enumFolder = "Assets/Scripts/Enums";

    public static UnityAction onAssetsRecompileCompleted;
    
    [UnityEditor.Callbacks.DidReloadScripts]
    static void onDidReloadScripts()
    {
        onAssetsRecompileCompleted?.Invoke();

    }
    /// <summary>
    /// Generates Enum in folder "Assets/Scripts/Enums" or in one what specified. 
    /// Returns UnityAction which is called when Assets recompiles and enum is generated.
    /// </summary>
    /// <param name="enumName"></param>
    /// <param name="enumEntries"></param>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public static UnityAction GenerateEnum (string enumName, string[] enumEntries, string filepath = "") {
        if(filepath == "")
        {
            filepath = enumFolder;
        }
        if (!Directory.Exists (filepath)) {
            Debug.LogError ("Directory " + filepath + " does not exist");
            return null;
        }
        filepath += "/" + enumName + ".cs";
        for(int i = 0; i < enumEntries.Length; i++)
        {
            enumEntries[i] = enumEntries[i].Replace(" ", "_");
            enumEntries[i] = enumEntries[i].Replace("-", "_");
        }
        using (StreamWriter streamWriter = new StreamWriter (filepath)) {
            streamWriter.WriteLine ("public enum " + enumName);
            streamWriter.WriteLine ("{");
            for (int i = 0; i < enumEntries.Length; i++) {
                //do not add coma after last
                if(i == enumEntries.Length - 1){
                    streamWriter.WriteLine ("\t" + enumEntries[i]);
                    continue;
                }
                streamWriter.WriteLine ("\t" + enumEntries[i] + ",");
            }
            streamWriter.WriteLine ("}");
        }

        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        return onAssetsRecompileCompleted;
    }
}
#endif