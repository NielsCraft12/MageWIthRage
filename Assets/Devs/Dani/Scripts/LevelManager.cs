using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Dependencies")]
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private LevelTransition _levelTransition;

    [Header("Game Settings")]
    [SerializeField]
    private float _levelTransitionDelay = 1f;

    [HideInInspector]
    public bool newGamePlus = false;
    public int currentLevel = 1;

    [HideInInspector]
    public int currentCheckpoint = 0;

    public int abilitiesUnlocked = 0;

    [Tooltip("Fill in the exact scene names in order. CASE SENSITIVE. Example: 'Level1'")]
    public List<string> levels = new List<string>();

    [SerializeField]
    private string scenePath = "Assets/Scenes/";

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
            newGamePlus = data.gameData.newGamePlus;
            currentLevel = data.gameData.currentLevel;
            currentCheckpoint = data.gameData.currentCheckpoint;
          //  abilitiesUnlocked = data.gameData.abilitiesUnlocked;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadLevel()
    {
        StartCoroutine(SceneLoad());
        // select
    }

    public void ToHome()
    {
        SceneManager.LoadScene("Menu");
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
            _gameManager = GameManager.instance;
            _gameManager.OnHook();
        }
    }

    private IEnumerator SceneLoad()
    {
        _levelTransition.FadeIn();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(
            scenePath + levels[currentLevel - 1]
        );

        asyncLoad.allowSceneActivation = false; // Prevent automatic activation

        // Wait for the delay


        yield return new WaitForSecondsRealtime(_levelTransitionDelay);

        // Allow scene activation after the delay
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
            _levelTransition.FadeOut();
        }
    }
}
