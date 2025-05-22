using UnityEngine;
using System.IO;
using System;

public static class SaveSystem
{
    public const string SaveFileName = "/savedata.json";

    public static void SaveGame()
    {
        string path = Application.persistentDataPath + SaveFileName;
        GameData gameData = new GameData(LevelManager.instance);
        SaveData saveData = new SaveData(gameData);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
    }
}

[Serializable]
public class SaveData
{
    [SerializeField] public GameData gameData;

    public SaveData(GameData levelData)
    {
        this.gameData = levelData;
    }
}

[Serializable]
public class GameData
{
    [SerializeField] public bool newGamePlus = false;
    [SerializeField] public int currentLevel = 0;
    [SerializeField] public int currentCheckpoint = 0;
    [SerializeField] public int abilitiesUnlocked = 0;

    public GameData(LevelManager levelManager)
    {
        newGamePlus = levelManager.newGamePlus;
        currentLevel = levelManager.currentLevel;
        currentCheckpoint = levelManager.currentCheckpoint;
    }

    // public GameData(Player player)
    // {
    //     abilitiesUnlocked = player.abilitiesUnlocked;
    // }
}
