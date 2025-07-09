using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas pauseMenuUI;

    // Auto-populated arrays (no need to assign manually)
    private GameObject[] cinemachineCameras;
    private MonoBehaviour[] cinemachineInputProviders;

    // Store original mouse sensitivity for fallback approach
    private float originalMouseSensitivity = 1f;
    private bool hasStoredSensitivity = false;

    [SerializeField]
    private PlayerCotroller playerController;

    [SerializeField]
    private PlayerLocalmotoininput playerInput;

    //    [SerializeField]
    //    private PlayerState playerState;

    private void Start()
    {
        // Auto-populate Cinemachine cameras and input providers
        PopulateCinemachineComponents();
    }

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
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible

        // Disable Cinemachine input providers only (keep cameras active for rendering)
        foreach (var inputProvider in cinemachineInputProviders)
        {
            if (inputProvider != null)
                inputProvider.enabled = false;
        }

        // Fallback: If no input providers found, disable mouse sensitivity on FreeLook cameras
        if (cinemachineInputProviders.Length == 0)
        {
            DisableMouseSensitivity();
        }

        // Disable player input
        if (playerController != null)
            playerController.SetPaused(true);
        if (playerInput != null)
            playerInput.SetPaused(true);
    }

    public void ResumeGame()
    {
        pauseMenuUI.enabled = false;
        Time.timeScale = 1f; // Resume the game
        //  playerState.CurrentPlayerMovementState = PlayerMovementState.Idling;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor

        // Re-enable Cinemachine input providers
        foreach (var inputProvider in cinemachineInputProviders)
        {
            if (inputProvider != null)
                inputProvider.enabled = true;
        }

        // Fallback: If no input providers found, restore mouse sensitivity
        if (cinemachineInputProviders.Length == 0)
        {
            RestoreMouseSensitivity();
        }

        // Re-enable player input
        if (playerController != null)
            playerController.SetPaused(false);
        if (playerInput != null)
            playerInput.SetPaused(false);
    }

    private void PopulateCinemachineComponents()
    {
        // Find all GameObjects with Cinemachine components
        var cameras = new System.Collections.Generic.List<GameObject>();
        var inputProviders = new System.Collections.Generic.List<MonoBehaviour>();

        // Find all MonoBehaviours in the scene
        MonoBehaviour[] allComponents = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var component in allComponents)
        {
            string typeName = component.GetType().Name;

            // Check if it's a Cinemachine camera component
            if (
                typeName.Contains("CinemachineFreeLook")
                || typeName.Contains("CinemachineVirtualCamera")
                || typeName.Contains("CinemachineCamera")
                || typeName.Contains("CinemachineBrain")
            )
            {
                if (!cameras.Contains(component.gameObject))
                {
                    cameras.Add(component.gameObject);
                }
            }

            // Check if it's a Cinemachine input provider
            if (
                typeName.Contains("CinemachineInputProvider")
                || typeName.Contains("InputProvider")
                || typeName.Contains("CinemachineInput")
            )
            {
                inputProviders.Add(component);
            }
        }

        // Convert lists to arrays
        cinemachineCameras = cameras.ToArray();
        cinemachineInputProviders = inputProviders.ToArray();

        // Debug info
        // Debug.Log(
        //     $"Auto-populated {cinemachineCameras.Length} Cinemachine cameras and {cinemachineInputProviders.Length} input providers"
        // );
    }

    private void DisableMouseSensitivity()
    {
        // Find FreeLook cameras and disable their mouse sensitivity
        foreach (var camera in cinemachineCameras)
        {
            if (camera != null)
            {
                var freeLook = camera.GetComponent<MonoBehaviour>();
                if (freeLook != null && freeLook.GetType().Name.Contains("CinemachineFreeLook"))
                {
                    // Use reflection to access X and Y axis properties
                    var xAxisField = freeLook.GetType().GetField("m_XAxis");
                    var yAxisField = freeLook.GetType().GetField("m_YAxis");

                    if (xAxisField != null && yAxisField != null)
                    {
                        var xAxis = xAxisField.GetValue(freeLook);
                        var yAxis = yAxisField.GetValue(freeLook);

                        if (xAxis != null && yAxis != null)
                        {
                            var maxSpeedFieldX = xAxis.GetType().GetField("m_MaxSpeed");
                            var maxSpeedFieldY = yAxis.GetType().GetField("m_MaxSpeed");

                            if (
                                maxSpeedFieldX != null
                                && maxSpeedFieldY != null
                                && !hasStoredSensitivity
                            )
                            {
                                // Store original sensitivity
                                originalMouseSensitivity = (float)maxSpeedFieldX.GetValue(xAxis);
                                hasStoredSensitivity = true;
                            }

                            if (maxSpeedFieldX != null && maxSpeedFieldY != null)
                            {
                                // Set sensitivity to 0
                                maxSpeedFieldX.SetValue(xAxis, 0f);
                                maxSpeedFieldY.SetValue(yAxis, 0f);
                            }
                        }
                    }
                }
            }
        }
    }

    private void RestoreMouseSensitivity()
    {
        // Find FreeLook cameras and restore their mouse sensitivity
        foreach (var camera in cinemachineCameras)
        {
            if (camera != null)
            {
                var freeLook = camera.GetComponent<MonoBehaviour>();
                if (freeLook != null && freeLook.GetType().Name.Contains("CinemachineFreeLook"))
                {
                    // Use reflection to access X and Y axis properties
                    var xAxisField = freeLook.GetType().GetField("m_XAxis");
                    var yAxisField = freeLook.GetType().GetField("m_YAxis");

                    if (xAxisField != null && yAxisField != null)
                    {
                        var xAxis = xAxisField.GetValue(freeLook);
                        var yAxis = yAxisField.GetValue(freeLook);

                        if (xAxis != null && yAxis != null)
                        {
                            var maxSpeedFieldX = xAxis.GetType().GetField("m_MaxSpeed");
                            var maxSpeedFieldY = yAxis.GetType().GetField("m_MaxSpeed");

                            if (maxSpeedFieldX != null && maxSpeedFieldY != null)
                            {
                                // Restore original sensitivity
                                maxSpeedFieldX.SetValue(xAxis, originalMouseSensitivity);
                                maxSpeedFieldY.SetValue(yAxis, originalMouseSensitivity);
                            }
                        }
                    }
                }
            }
        }
    }
}
