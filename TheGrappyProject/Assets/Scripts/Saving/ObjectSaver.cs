using UnityEngine;

public static class ObjectSaver<T>
{
    public static void SaveObject(T objectToSave, 
        string fileName)
    {
        string json = JsonUtility.ToJson(objectToSave);
        PlayerPrefs.SetString(fileName, json);
    }
    public static T LoadObject(string fileName)
    {
        string json = PlayerPrefs.GetString(fileName);
        T loadedObject = JsonUtility.FromJson<T>(json);
        return loadedObject;
    }
}
