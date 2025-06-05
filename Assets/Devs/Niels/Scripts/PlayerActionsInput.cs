using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerActionsnput : MonoBehaviour, PlayerControls.IActionsActions
{
    private PlayerLocalmotoininput playerLocalMotoinInput;
    private PlayerState playerState;
    public PlayerControls PlayerControls { get; private set; }
    public bool AttackPressed { get; set; }

    MemoryUse memoryUse;

    private void Awake()
    {
        playerLocalMotoinInput = GetComponent<PlayerLocalmotoininput>();
        playerState = GetComponent<PlayerState>();
        memoryUse = GetComponent<MemoryUse>();
    }

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

    private void Update()
    {
        if (
            playerLocalMotoinInput.MovementInput != Vector2.zero
            || playerState.CurrentPlayerMovementState == PlayerMovementState.Jumping
            || playerState.CurrentPlayerMovementState == PlayerMovementState.Falling
        )
        {
            AttackPressed = false; // Reset attack pressed if player is moving or jumping
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (LevelManager.instance.abilitiesUnlocked < 3)
        {
            return; // Prevent jumping if the ability is not unlocked
        }
        if (!context.performed)
            return;
        AttackPressed = true;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        memoryUse.UseNewestMemory();
    }
}
