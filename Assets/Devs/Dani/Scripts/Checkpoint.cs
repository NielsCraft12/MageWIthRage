using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameManager gameManager;
    [Tooltip("Starts at 1, 0 is ignored")][SerializeField] private int checkpointIndex;

    public Transform spawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && gameManager.currentCheckpoint < checkpointIndex)
        {
            gameManager.currentCheckpoint = checkpointIndex;
            LevelManager.instance.currentCheckpoint = checkpointIndex;
            Debug.Log("Checkpoint reached: " + checkpointIndex);
        }
    }
}
