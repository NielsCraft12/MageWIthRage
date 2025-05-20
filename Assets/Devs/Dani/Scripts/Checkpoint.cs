using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private int checkpointIndex;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            gameManager.currentCheckpoint = checkpointIndex;
            LevelManager.instance.currentCheckpoint = checkpointIndex;
            Debug.Log("Checkpoint reached: " + checkpointIndex);
        }
    }
}
