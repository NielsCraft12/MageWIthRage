using UnityEngine;

public class SimplePlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDamage = 50f;
    public float attackRange = 2f;
    public LayerMask enemyLayer = -1; // All layers by default
    public LayerMask breakableWallLayer = -1; // Layer for breakable walls

    [Header("References")]
    public Animator animator;
    public Transform attackPoint; // Where the attack originates from

    private PlayerActionsnput playerInput;
    private bool canAttack = true;
    private bool isSecondAttack = false;

    void Start()
    {
        playerInput = GetComponent<PlayerActionsnput>();

        // If no attack point is set, use the player's position
        if (attackPoint == null)
            attackPoint = transform;
    }

    void Update()
    {
        // Check for attack input
        if (canAttack && playerInput.BonkPressed)
        {
            isSecondAttack = false;
            StartAttack();
        }
        else if (canAttack && playerInput.BonkLvl1Pressed)
        {
            isSecondAttack = true;
            StartAttack();
        }
    }

    void StartAttack()
    {
        canAttack = false;
        // The actual attack damage will be triggered by animation event
        // Animation will call "TriggerAttackDamage" when the attack should hit
    }

    // This method is called by animation event during the attack animation
    public void TriggerAttackDamage()
    {
        // Find all enemies in attack range
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in enemies)
        {
            // Check if the enemy has a Health component
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log($"Player attacked {enemy.name} for {attackDamage} damage!");
            }
        }

        // Check for breakable walls (works with both regular and second attack)
        Collider[] breakableWalls = Physics.OverlapSphere(
            attackPoint.position,
            attackRange,
            breakableWallLayer
        );

        foreach (Collider wall in breakableWalls)
        {
            // Check for the new BreakableWall2 component first
            BreakableWall2 breakableWall2 = wall.GetComponent<BreakableWall2>();
            if (breakableWall2 != null)
            {
                breakableWall2.TakeDamage(attackDamage);
                Debug.Log(
                    $"Player attacked breakable wall: {wall.name} for {attackDamage} damage!"
                );
                continue;
            }

            // Check for the old newBreakableWall component
            newBreakableWall breakableWall = wall.GetComponent<newBreakableWall>();
            if (breakableWall != null)
            {
                breakableWall.Break();
                Debug.Log($"Player destroyed breakable wall: {wall.name}");
                continue;
            }

            // For walls that need to be destroyed instantly on second attack
            if (isSecondAttack)
            {
                // Fallback: just destroy the GameObject
                Destroy(wall.gameObject);
                Debug.Log($"Player destroyed wall: {wall.name}");
            }
        }
    }

    // This method is called by animation event when attack animation ends
    public void EndAttack()
    {
        canAttack = true;
    }

    // Draw attack range in scene view
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
