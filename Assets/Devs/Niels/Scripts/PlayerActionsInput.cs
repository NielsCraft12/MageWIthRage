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

    [Header("Attack System")]
    public float attackDamage = 50f;
    public float attackRange = 2f;
    public LayerMask enemyLayer = -1;
    public Transform attackPoint; // Where the attack originates from

    private bool canAttack = true;
    private bool isAttacking = false; // Track if we're currently in an attack

    private void Awake()
    {
        playerLocalMotoinInput = GetComponent<PlayerLocalmotoininput>();
        playerState = GetComponent<PlayerState>();
        memoryUse = GetComponent<MemoryUse>();

        // If no attack point is set, use the player's position
        if (attackPoint == null)
            attackPoint = transform;
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
        if (LevelManager.instance != null && LevelManager.instance.abilitiesUnlocked < 3)
        {
            return; // Prevent attacking if the ability is not unlocked
        }
        if (!context.performed)
            return;

        // Only allow attack if we can attack
        if (!canAttack)
            return;

        // Keep existing attack system for compatibility
        if (attackActive == 0)
        {
            BonkLvl1Pressed = true;
        }
        else if (attackActive == 1)
        {
            BonkPressed = true;
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
        Debug.Log("Player is attacking!");
        // The actual attack damage will be triggered by animation event
        // Animation will call "TriggerAttackDamage" when the attack should hit
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

        // Find all enemies in attack range
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        Debug.Log($"Player attack found {enemies.Length} potential targets in range");

        foreach (Collider enemy in enemies)
        {
            // Skip if it's the player itself
            if (enemy.gameObject == gameObject)
            {
                Debug.Log("Skipping player self");
                continue;
            }

            // Check if it's an enemy (has Enemy tag or Slime component)
            bool isEnemy = enemy.CompareTag("Enemy") || enemy.GetComponent<Slime>() != null;

            if (isEnemy)
            {
                // Check if the enemy has a Health component
                Health enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                    Debug.Log($"Player attacked {enemy.name} for {attackDamage} damage!");
                }
                else
                {
                    Debug.Log($"Enemy {enemy.name} has no Health component");
                }
            }
            else
            {
                Debug.Log($"Object {enemy.name} is not an enemy");
            }
        }
    }

    /// <summary>
    /// This method is called by animation event when attack animation ends
    /// </summary>
    public void EndAttack()
    {
        canAttack = true;
        isAttacking = false;
        Debug.Log("Attack finished, can attack again");
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

    #endregion
}
