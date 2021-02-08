using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationManager
{
    public static string GetSavePath()
    {
        return Application.persistentDataPath + "/saves/";
    }
    public static bool Save(string saveName, object saveData, string pathFromSaveFolder = null)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string savePath = GetSavePath();
        if (pathFromSaveFolder != null)
        {
            savePath += pathFromSaveFolder;
        }
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        string path = savePath + saveName + ".save";
        FileStream stream = File.Create(path);
        binaryFormatter.Serialize(stream, saveData);
        stream.Close();
        return true;
    }
    public static object Load(string saveName, string pathFromSaveFolder = null)
    {
        string path = GetSavePath();
        if(pathFromSaveFolder != null)
        {
            path += pathFromSaveFolder;
        }
        path += saveName + ".save";
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = File.Open(path, FileMode.Open);

        try
        {
            object saveObj = binaryFormatter.Deserialize(stream);
            stream.Close();
            return saveObj;
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load file at {0}", path);
            stream.Close();
            return null;
        }
    }

}
