using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager<T> where T : SaveData, new()
{
    static readonly string savePath = Application.persistentDataPath;

    static T _loadedSave;

    public static void Save(T whatToSave)
    {
        string json = JsonUtility.ToJson(whatToSave, true);
        File.WriteAllText(savePath + GetFileName(typeof(T)), json);

        _loadedSave = whatToSave;
    }

    public static T Load()
    {
        if (File.Exists(savePath + GetFileName(typeof(T))))
        {
            string json = File.ReadAllText(savePath + GetFileName(typeof(T)));
            _loadedSave = JsonUtility.FromJson<T>(json);
        } else 
        {
            T newSave = new T();
            Save(newSave);
        }

        return _loadedSave;
    }

    static string GetFileName(Type type) => "/" + type + ".data";
}
