using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Dependencies")]
    private LevelManager levelManager;
    [SerializeField] private GameObject player;
    public Transform[] checkpoints;
    [HideInInspector] public int currentCheckpoint;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void OnHook()
    {
        levelManager = LevelManager.instance;
        currentCheckpoint = LevelManager.instance.currentCheckpoint;
        if (currentCheckpoint != 0)
        {
            player.transform.position = checkpoints[currentCheckpoint - 1].GetComponent<Checkpoint>().spawnPoint.position;
        }
    }
}
