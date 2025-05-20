using UnityEngine;

public class LevelExit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.name != "Player")
            return;
        if (LevelManager.instance.levels.Count <= LevelManager.instance.currentLevel)
        {
            Debug.Log("No more levels to load.");
            return;
        }
        LevelManager.instance.currentLevel++;
        LevelManager.instance.currentCheckpoint = 0;
        Debug.Log("Level exit reached.");
        LevelManager.instance.LoadLevel();
    }
}
