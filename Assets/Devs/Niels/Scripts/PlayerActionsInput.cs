using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerActionsnput : MonoBehaviour, PlayerControls.IActionsActions
{
    private PlayerLocalmotoininput playerLocalMotoinInput;
    private PlayerState playerState;
    public PlayerControls PlayerControls { get; private set; }
    public bool BonkPressed { get; set; }
    public bool BonkLvl1Pressed { get; set; }
    public int attackActive;

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
        // if (
        //     playerLocalMotoinInput.MovementInput != Vector2.zero
        //     || playerState.CurrentPlayerMovementState == PlayerMovementState.Jumping
        //     || playerState.CurrentPlayerMovementState == PlayerMovementState.Falling
        // )
        // {
        //     AttackPressed = false; // Reset attack pressed if player is moving or jumping
        // }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (LevelManager.instance.abilitiesUnlocked < 3)
        {
            return; // Prevent attacking if the ability is not unlocked
        }
        if (!context.performed)
            return;

        if (attackActive == 0)
        {
            BonkLvl1Pressed = true;
        }
        else if (attackActive == 1)
        {
            BonkPressed = true;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (memoryUse != null)
        {
            memoryUse.UseNewestMemory();
        }
        else
        {
            Debug.LogWarning("MemoryUse component not found on this GameObject!");
        }
    }
}
