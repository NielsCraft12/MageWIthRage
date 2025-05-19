using UnityEngine;
using System.IO;

public static class LoadSystem
{
    public static SaveData LoadGameData()
    {
        try
        {
            string path = Application.persistentDataPath + SaveSystem.SaveFileName;
            string fileContent = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(fileContent);
            return saveData;
        }
        catch (FileNotFoundException)
        {
            Debug.LogError("Save file not found. Starting a new game.");
            return null;
        }
        catch (IOException e)
        {
            Debug.LogError($"Error reading save file: {e.Message}");
            return null;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"An unexpected error occurred: {e.Message}");
            return null;
        }
    }
}
