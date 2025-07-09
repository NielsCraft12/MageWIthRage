using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerActionsnput
    : MonoBehaviour,
        PlayerControls.IActionsActions,
        PlayerControls.IUIActions
{
    private PlayerLocalmotoininput playerLocalMotoinInput;
    private PlayerState playerState;
    public PlayerControls PlayerControls { get; private set; }
    public bool BonkPressed { get; set; }
    public bool BonkLvl1Pressed { get; set; }
    public int attackActive;

    MemoryUse memoryUse;

    [Header("Attack System")]
    public float attackDamage = 50f;
    public float attackRange = 2f;
    public LayerMask enemyLayer = -1;
    public Transform attackPoint; // Where the attack originates from

    private bool canAttack = true;
    private bool isAttacking = false; // Track if we're currently in an attack

    [Header("Attack Selection")]
    public int maxAttackTypes = 2; // Number of available attack types

    private void Awake()
    {
        playerLocalMotoinInput = GetComponent<PlayerLocalmotoininput>();
        playerState = GetComponent<PlayerState>();
        memoryUse = GetComponent<MemoryUse>();

        // If no attack point is set, use the player's position
        if (attackPoint == null)
            attackPoint = transform;

        // Debug component validation
        if (playerLocalMotoinInput == null)
            Debug.LogError("PlayerLocalmotoininput component not found!");
        if (playerState == null)
            Debug.LogError("PlayerState component not found!");
        if (memoryUse == null)
            Debug.LogWarning("MemoryUse component not found!");

        // Ensure player starts with an unlocked attack
        EnsureValidAttackSelection();
    }

    void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.Actions.Enable();
        PlayerControls.Actions.SetCallbacks(this);

        PlayerControls.UI.Enable();
        PlayerControls.UI.SetCallbacks(this);
    }

    void OnDisable()
    {
        PlayerControls.Disable();
        PlayerControls.Actions.RemoveCallbacks(this);
        PlayerControls.UI.RemoveCallbacks(this);
    }

    private void Update()
    {
        // Debug key bindings for testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Manual reset triggered!");
            ForceResetAttack();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            DebugAttackState();
        }

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
        Debug.Log(
            $"OnAttack called - context.performed: {context.performed}, canAttack: {canAttack}, isAttacking: {isAttacking}"
        );

        if (!context.performed)
            return;

        // Only allow attack if we can attack
        if (!canAttack)
        {
            Debug.Log("Attack blocked - canAttack is false");
            return;
        }

        // Keep existing attack system for compatibility
        if (attackActive == 0)
        {
            BonkLvl1Pressed = true;
            Debug.Log("Setting BonkLvl1Pressed = true (Attack Type 0)");
        }
        else if (attackActive == 1)
        {
            // Check if LevelManager exists and abilities are unlocked
            if (LevelManager.instance != null && LevelManager.instance.abilitiesUnlocked >= 3)
            {
                BonkPressed = true;
                Debug.Log("Setting BonkPressed = true (Attack Type 1)");
            }
            else
            {
                Debug.Log("Attack Type 1 selected but not unlocked yet or LevelManager not found!");
                return; // Don't start attack if not unlocked
            }
        }

        // Start attack
        StartAttack();
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

    #region Attack System

    void StartAttack()
    {
        canAttack = false;
        isAttacking = true;

        Debug.Log(
            $"Player is attacking with attack type {attackActive} ({GetAttackTypeName(attackActive)})!"
        );

        // The actual attack damage will be triggered by animation event
        // Animation will call "TriggerAttackDamage" when the attack should hit
        // IdleManager will handle calling EndAttack when animation finishes
    }

    /// <summary>
    /// This method is called by animation event during the attack animation
    /// </summary>
    public void TriggerAttackDamage()
    {
        // Only damage if we're actually in an attack state
        if (!isAttacking)
        {
            Debug.Log("TriggerAttackDamage called but player is not attacking - ignoring");
            return;
        }

        // Find all targets in attack range
        Collider[] targets = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        Debug.Log($"Player attack found {targets.Length} potential targets in range");

        foreach (Collider target in targets)
        {
            // Skip if it's the player itself
            if (target.gameObject == gameObject)
            {
                Debug.Log("Skipping player self");
                continue;
            }

            // Check if it's an enemy (has Enemy tag or Slime component)
            bool isEnemy = target.CompareTag("Enemy") || target.GetComponent<Slime>() != null;

            if (isEnemy)
            {
                // Check if the enemy has a Health component
                Health enemyHealth = target.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                    Debug.Log($"Player attacked {target.name} for {attackDamage} damage!");
                }
                else
                {
                    Debug.Log($"Enemy {target.name} has no Health component");
                }
            }
            else
            {
                // Check if it's a breakable wall
                BreakableWall2 breakableWall2 = target.GetComponent<BreakableWall2>();
                newBreakableWall newBreakableWall = target.GetComponent<newBreakableWall>();

                if (breakableWall2 != null)
                {
                    // Debug: Log current attack type and damage
                    Debug.Log(
                        $"BreakableWall2 found! attackActive: {attackActive}, attackDamage: {attackDamage}"
                    );

                    // For BreakableWall2, try damage system first, fall back to instant break
                    if (attackActive == 1) // Second attack type for instant break
                    {
                        breakableWall2.Break();
                        Debug.Log(
                            "Player broke wall " + target.name + " instantly with second attack!"
                        );
                    }
                    // else
                    // {
                    //     // Try damage system
                    //     Debug.Log($"Calling TakeDamage({attackDamage}) on {target.name}");
                    //     breakableWall2.TakeDamage(attackDamage);
                    //     Debug.Log(
                    //         "Player attacked breakable wall "
                    //             + target.name
                    //             + " for "
                    //             + attackDamage
                    //             + " damage!"
                    //     );
                    // }
                }
                else if (newBreakableWall != null)
                {
                    if (attackActive == 1)
                    {
                        newBreakableWall.Break();
                        Debug.Log($"Player broke wall {target.name} instantly!");
                    }
                    // For newBreakableWall, use the instant break method
                }
                else
                {
                    Debug.Log($"Object {target.name} is not an enemy or breakable wall");
                }
            }
        }
    }

    /// <summary>
    /// Manual reset function - call this if attack gets stuck
    /// </summary>
    public void ForceResetAttack()
    {
        Debug.Log("Force resetting attack state!");
        // Reset the attack state directly since EndAttack is moved to IdleManager
        canAttack = true;
        isAttacking = false;
        BonkPressed = false;
        BonkLvl1Pressed = false;
    }

    /// <summary>
    /// Draw attack range in scene view for debugging
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

    /// <summary>
    /// Alternative animation event method for starting attack damage window
    /// </summary>
    public void StartAttackDamageWindow()
    {
        Debug.Log("Attack damage window started via animation event");
        // You can add specific logic here for when damage should start
    }

    /// <summary>
    /// Alternative animation event method for ending attack damage window
    /// </summary>
    public void EndAttackDamageWindow()
    {
        Debug.Log("Attack damage window ended via animation event");
        // You can add specific logic here for when damage should end
    }

    /// <summary>
    /// Animation event method to reset attack state at any point
    /// </summary>
    public void ResetAttackState()
    {
        Debug.Log("Attack state reset via animation event");
        ForceResetAttack();
    }

    /// <summary>
    /// Public method for IdleManager to reset attack state
    /// </summary>
    public void EndAttackFromIdleManager()
    {
        canAttack = true;
        isAttacking = false;
        BonkPressed = false;
        BonkLvl1Pressed = false;
        Debug.Log("Attack ended by IdleManager");
    }

    #endregion

    #region UI Actions Interface Implementation

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Vector2 scrollValue = context.ReadValue<Vector2>();

        // Scroll up (positive Y) to go to next attack, scroll down (negative Y) to go to previous
        if (scrollValue.y > 0)
        {
            CycleAttackType(1); // Next attack
        }
        else if (scrollValue.y < 0)
        {
            CycleAttackType(-1); // Previous attack
        }
    }

    // Empty implementations for required UI interface methods
    public void OnNavigate(InputAction.CallbackContext context) { }

    public void OnSubmit(InputAction.CallbackContext context) { }

    public void OnCancel(InputAction.CallbackContext context) { }

    public void OnPoint(InputAction.CallbackContext context) { }

    public void OnClick(InputAction.CallbackContext context) { }

    public void OnMiddleClick(InputAction.CallbackContext context) { }

    public void OnRightClick(InputAction.CallbackContext context) { }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }

    #endregion

    #region Attack Selection System

    void CycleAttackType(int direction)
    {
        int originalAttackActive = attackActive;
        int attempts = 0;

        do
        {
            // Cycle through attack types
            attackActive = (attackActive + direction) % maxAttackTypes;

            // Handle negative wrap-around
            if (attackActive < 0)
                attackActive = maxAttackTypes - 1;

            attempts++;

            // Prevent infinite loop if no attacks are available
            if (attempts >= maxAttackTypes)
            {
                attackActive = originalAttackActive;
                Debug.Log("No unlocked attacks available to cycle to!");
                return;
            }
        } while (!IsAttackUnlocked(attackActive));

        // Give feedback to player about current attack
        Debug.Log($"Attack type changed to: {GetAttackTypeName(attackActive)}");

        // You could also add UI feedback here, like updating a UI element
        // or playing a sound effect
    }

    string GetAttackTypeName(int attackType)
    {
        string baseName;
        switch (attackType)
        {
            case 0:
                baseName = "Bonk Level 1";
                break;
            case 1:
                baseName = "Bonk (Advanced)";
                break;
            default:
                baseName = "Unknown Attack";
                break;
        }

        // Add lock status for better feedback
        if (!IsAttackUnlocked(attackType))
        {
            return $"{baseName} [LOCKED]";
        }

        return baseName;
    }

    bool IsAttackUnlocked(int attackType)
    {
        switch (attackType)
        {
            case 0:
                return true; // Bonk Level 1 is always unlocked
            case 1:
                // Check if LevelManager exists and advanced attack is unlocked
                return LevelManager.instance != null
                    && LevelManager.instance.abilitiesUnlocked >= 3;
            default:
                return false; // Unknown attack types are locked
        }
    }

    void EnsureValidAttackSelection()
    {
        // If current attack is not unlocked, find the first unlocked attack
        if (!IsAttackUnlocked(attackActive))
        {
            for (int i = 0; i < maxAttackTypes; i++)
            {
                if (IsAttackUnlocked(i))
                {
                    attackActive = i;
                    Debug.Log(
                        $"Attack selection reset to unlocked attack: {GetAttackTypeName(attackActive)}"
                    );
                    return;
                }
            }

            // If no attacks are unlocked, default to attack 0 (should always be unlocked)
            attackActive = 0;
            Debug.LogWarning("No unlocked attacks found, defaulting to attack 0");
        }
    }

    /// <summary>
    /// Call this method when abilities are unlocked to refresh attack selection
    /// </summary>
    public void RefreshAttackAvailability()
    {
        EnsureValidAttackSelection();
        Debug.Log(
            $"Attack availability refreshed. Current attack: {GetAttackTypeName(attackActive)}"
        );
    }

    #endregion

    /// <summary>
    /// Debug method to check current attack state
    /// </summary>
    public void DebugAttackState()
    {
        Debug.Log($"=== Attack State Debug ===");
        Debug.Log($"canAttack: {canAttack}");
        Debug.Log($"isAttacking: {isAttacking}");
        Debug.Log($"BonkPressed: {BonkPressed}");
        Debug.Log($"BonkLvl1Pressed: {BonkLvl1Pressed}");
        Debug.Log($"attackActive: {attackActive}");
        Debug.Log($"Current Attack: {GetAttackTypeName(attackActive)}");
        Debug.Log($"=========================");
    }
}
