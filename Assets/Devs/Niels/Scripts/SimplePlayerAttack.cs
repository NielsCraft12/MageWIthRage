using UnityEngine;

public class SimplePlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDamage = 50f;
    public float attackRange = 2f;
    public LayerMask enemyLayer = -1; // All layers by default

    [Header("References")]
    public Animator animator;
    public Transform attackPoint; // Where the attack originates from

    private PlayerActionsnput playerInput;
    private bool canAttack = true;

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
        if (canAttack && (playerInput.BonkPressed || playerInput.BonkLvl1Pressed))
        {
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
