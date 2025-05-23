using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerActionsnput : MonoBehaviour, PlayerControls.IActionsActions
{
    public PlayerControls PlayerControls { get; private set; }
    public bool AttackPressed { get; private set; }

    void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.Actions.Enable();
        PlayerControls.Actions.SetCallbacks(this);
    }

    void OnDisable()
    {
        PlayerControls.Disable();
        PlayerControls.Actions.RemoveCallbacks(this);
    }

    void LateUpdate()
    {
        AttackPressed = false;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        AttackPressed = true;
    }
}
