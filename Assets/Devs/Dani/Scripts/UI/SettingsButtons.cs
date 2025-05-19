using UnityEngine;

public class SettingsButtons : MonoBehaviour
{
    public void WipeData()
    {
        string path = Application.persistentDataPath + SaveSystem.SaveFileName;
        if (System.IO.File.Exists(path))
        {
            LevelManager.instance.currentLevel = 0;
            LevelManager.instance.newGamePlus = false;
            System.IO.File.Delete(path);
            Debug.Log("Save data wiped successfully.");
        }
        else
        {
            Debug.Log("No save data found to wipe.");
        }
    }
}
