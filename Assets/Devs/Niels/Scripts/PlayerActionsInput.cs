using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerActionsnput : MonoBehaviour, PlayerControls.IActionsActions
{
    public PlayerControls PlayerControls { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool InteractPressed { get; private set; }

    private MemoryUse memoryUse;

    void OnEnable()
    {
        memoryUse = GetComponent<MemoryUse>();
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
        InteractPressed = true;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        AttackPressed = true;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        InteractPressed = true;
        memoryUse.UseNewestMemory();
    }
}
