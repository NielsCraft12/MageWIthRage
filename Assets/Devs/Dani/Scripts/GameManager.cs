using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Dependencies")]
    private LevelManager levelManager;
    [SerializeField] private GameObject player;

    [SerializeField] private Transform[] checkpoints;
    [SerializeField] public int currentCheckpoint;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void OnHook()
    {
        levelManager = LevelManager.instance;
        int currentCheckpoint = LevelManager.instance.currentCheckpoint;
        if (currentCheckpoint != 0)
        {
            player.transform.position = checkpoints[currentCheckpoint - 1].position;
        }
    }
}
