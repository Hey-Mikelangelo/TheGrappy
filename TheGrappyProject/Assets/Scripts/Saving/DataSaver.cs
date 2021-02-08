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
    public static string InitSaveFolder()
    {
        string path = GetDefaultPath();
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    public static string GetDefaultPath()
    {
        return Application.persistentDataPath + "/Saves/";
    }
    public static void Save(T structToSave, string fileName, string path = null)
    {
        if (path == null)
        {
            path = GetDefaultPath();
        }
        string pathToFile = path + fileName;
        if (!File.Exists(pathToFile))
        {
            File.Create(pathToFile);
        }
        fileStream = new FileStream(pathToFile, FileMode.Create);

        binaryFormatter.Serialize(fileStream, structToSave);
        fileStream.Close();
    }
    public static T Load(string fileName, string path = null)
    {
        if (path == null)
        {
            path = GetDefaultPath();
        }
        if (File.Exists(path + fileName))
        {
            T data;
            fileStream = new FileStream(path + fileName, FileMode.Open);
            if (fileStream.Length > 0)
            {
                data = (T)binaryFormatter.Deserialize(fileStream);
            }
            else
            {
                data = new T();
            }
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
