using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocalmotoininput : MonoBehaviour, PlayerControls.IPlayerActions
{
    [SerializeField]
    private bool holdToSprint = true;

    public bool SprintToggleOn { get; private set; }
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.Player.Enable();
        PlayerControls.Player.SetCallbacks(this);
    }

    void OnDisable()
    {
        PlayerControls.Disable();
        PlayerControls.Player.RemoveCallbacks(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnToggleSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SprintToggleOn = holdToSprint || !SprintToggleOn;
        }
        else if (context.canceled)
        {
            SprintToggleOn = !holdToSprint || SprintToggleOn;
        }
    }
}
