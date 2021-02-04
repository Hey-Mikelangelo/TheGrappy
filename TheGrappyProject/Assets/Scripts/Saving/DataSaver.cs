using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public static class DataSaver<T> where T : struct
{
    static BinaryFormatter binaryFormatter = new BinaryFormatter();
    static FileStream fileStream;

    static void SetDefaultPath(out string path)
    {
        path = Application.persistentDataPath + "/Saves/";
    }
    public static void Save(T structToSave, string fileName, string path = null)
    {
        if(path == null)
        {
            SetDefaultPath(out path);
        }
        string pathToFile = path + fileName;
        if (!File.Exists(pathToFile))
        {
            File.Create(pathToFile);
        }
        fileStream = new FileStream(pathToFile, FileMode.Open, FileAccess.Write);
        binaryFormatter.Serialize(fileStream, structToSave);
        fileStream.Close();
    }
    public static T Load(string fileName, string path = null)
    {
        if (path == null)
        {
            SetDefaultPath(out path);
        }
        if (File.Exists(path+fileName))
        {
            fileStream = new FileStream(path + fileName, FileMode.Open, FileAccess.Read);
            T data = (T)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return data;
        }
        else
        {
            Debug.LogError("No file: " + path + fileName);
            return default;
        }
       
    }
}
