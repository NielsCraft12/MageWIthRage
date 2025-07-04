using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas pauseMenuUI;

    //    [SerializeField]
    //    private PlayerState playerState;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenuUI.isActiveAndEnabled)
        {
            ResumeGame();
        }
        else
        {
            OpenPauseMenu();
        }
    }

    public void OpenPauseMenu()
    {
        pauseMenuUI.enabled = true;
        Time.timeScale = 0f; // Pause the game
        // playerState.CurrentPlayerMovementState = PlayerMovementState.Paused;
    }

    public void ResumeGame()
    {
        pauseMenuUI.enabled = false;
        Time.timeScale = 1f; // Resume the game
        //  playerState.CurrentPlayerMovementState = PlayerMovementState.Idling;
    }
}
