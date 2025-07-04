using UnityEngine;

/// <summary>
/// The simplest possible attack system for your game.
/// Attach this to your Player GameObject.
/// </summary>
public class SuperSimpleAttack : MonoBehaviour
{
    [Header("Player Attack")]
    public float playerDamage = 50f;
    public float attackRange = 2f;

    [Header("References")]
    public Animator playerAnimator;

    private PlayerActionsnput input;
    private bool canAttack = true;

    void Start()
    {
        input = GetComponent<PlayerActionsnput>();
    }

    void Update()
    {
        // Check if player wants to attack
        if (canAttack && (input.BonkPressed || input.BonkLvl1Pressed))
        {
            Attack();
        }
    }

    void Attack()
    {
        canAttack = false;
        Debug.Log("Player is attacking!");

        // Animation will call DealDamage() when the attack should hit
        // Add this to your attack animation as an Animation Event
    }

    /// <summary>
    /// Call this method from your attack animation event
    /// </summary>
    public void DealDamage()
    {
        // Find all enemies near the player
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider enemy in enemies)
        {
            // Check if it's a slime
            if (enemy.CompareTag("Enemy") || enemy.GetComponent<Slime>() != null)
            {
                Health enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(playerDamage);
                    Debug.Log("Hit enemy for " + playerDamage + " damage!");
                }
            }
        }
    }

    /// <summary>
    /// Call this method from your attack animation event when attack ends
    /// </summary>
    public void EndAttack()
    {
        canAttack = true;
        Debug.Log("Attack finished, can attack again");
    }

    // Draw attack range in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

/// <summary>
/// The simplest possible slime attack system.
/// Attach this to your Slime GameObject.
/// </summary>
public class SuperSimpleSlimeAttack : MonoBehaviour
{
    [Header("Slime Attack")]
    public float slimeDamage = 10f;
    public float attackCooldown = 2f;

    [Header("References")]
    public Animator slimeAnimator;
    public string attackTrigger = "Attack";

    private bool canAttack = true;
    private GameObject player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canAttack)
        {
            player = other.gameObject;
            AttackPlayer();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            player = collision.gameObject;
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        canAttack = false;

        // Play attack animation
        if (slimeAnimator != null)
        {
            slimeAnimator.SetTrigger(attackTrigger);
        }

        Debug.Log("Slime is attacking player!");

        // Wait for cooldown
        Invoke("ResetAttack", attackCooldown);
    }

    /// <summary>
    /// Call this method from your slime attack animation event
    /// </summary>
    public void DamagePlayer()
    {
        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(slimeDamage);
                Debug.Log("Slime hit player for " + slimeDamage + " damage!");
            }
        }
    }

    void ResetAttack()
    {
        canAttack = true;
        player = null;
    }
}
