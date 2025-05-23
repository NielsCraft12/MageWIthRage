using UnityEngine;

public class LevelExit : MonoBehaviour
{
    LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.instance;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.name != "Player")
            return;
        if (LevelManager.instance.levels.Count <= LevelManager.instance.currentLevel)
        {
            Debug.Log("No more levels to load.");
            levelManager.currentLevel = 0;
            levelManager.currentCheckpoint = 0;
            levelManager.ToHome();
            return;
        }
        LevelManager.instance.currentLevel++;
        LevelManager.instance.currentCheckpoint = 0;
        Debug.Log("Level exit reached.");
        LevelManager.instance.LoadLevel();
    }
}
