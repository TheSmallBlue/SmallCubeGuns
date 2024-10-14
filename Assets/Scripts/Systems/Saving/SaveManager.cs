using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    static readonly string savePath = Application.persistentDataPath;

    static SaveData _loadedSave;

    public static void Save<T>(T whatToSave) where T : SaveData
    {
        string json = JsonUtility.ToJson(whatToSave, true);
        File.WriteAllText(savePath, json);

        _loadedSave = whatToSave;
    }

    public static T Load<T>() where T : SaveData, new()
    {
        if (!File.Exists(savePath)) return new T();

        string json = File.ReadAllText(savePath);
        _loadedSave = JsonUtility.FromJson<T>(json);

        return (T)_loadedSave;
    }
}
