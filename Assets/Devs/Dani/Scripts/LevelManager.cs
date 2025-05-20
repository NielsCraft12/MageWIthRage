using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Dependencies")]
    [SerializeField] private GameManager gameManager;

    [Header("Game Settings")]
    [HideInInspector] public bool newGamePlus = false;
    [HideInInspector] public int currentLevel = 1;
    [HideInInspector] public int currentCheckpoint = 0;


    [Tooltip("Fill in the exact scene names in order. CASE SENSITIVE")] public List<string> levels = new List<string>();
    [SerializeField] private string scenePath = "Assets/Scenes/";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SaveData data = LoadSystem.LoadGameData();

        if (data != null)
        {
            newGamePlus = data.levelData.newGamePlus;
            currentLevel = data.levelData.currentLevel;
            currentCheckpoint = data.levelData.currentCheckpoint;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(scenePath + levels[currentLevel - 1]);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveSystem.SaveGame();
        }
    }

    private void OnApplicationQuit()
    {
        SaveSystem.SaveGame();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == levels[currentLevel - 1])
        {
            Debug.Log("Level loaded: " + scene.name);
            gameManager = GameManager.instance;
            gameManager.OnHook();
        }
    }
}
