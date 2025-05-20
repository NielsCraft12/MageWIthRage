using UnityEngine;
using System.IO;
using System;

public static class SaveSystem
{
    public const string SaveFileName = "/savedata.json";

    public static void SaveGame()
    {
        string path = Application.persistentDataPath + SaveFileName;
        LevelData levelData = new LevelData(LevelManager.instance);
        SaveData saveData = new SaveData(levelData);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
    }
}

[Serializable]
public class SaveData
{
    [SerializeField] public LevelData levelData;

    public SaveData(LevelData levelData)
    {
        this.levelData = levelData;
    }
}

[Serializable]
public class LevelData
{
    [SerializeField] public bool newGamePlus = false;
    [SerializeField] public int currentLevel = 0;
    [SerializeField] public int currentCheckpoint = 0;

    public LevelData(LevelManager levelManager)
    {
        newGamePlus = levelManager.newGamePlus;
        currentLevel = levelManager.currentLevel;
        currentCheckpoint = levelManager.currentCheckpoint;
    }
}
