#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;

public class EnumGenerator {
    //if called without filepath, enum class will be created in EnumGenerator.enumsDirectory
    public static void GenerateEnum (string enumName, string filepath, string[] enumEntries) {
        
        if (!Directory.Exists (filepath)) {
            Debug.LogError ("Directory " + filepath + " does not exist");
            return;
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
        AssetDatabase.Refresh ();
        Debug.Log("Created file");
    }
}
#endif