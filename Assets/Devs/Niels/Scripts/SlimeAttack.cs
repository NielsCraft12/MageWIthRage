using UnityEngine;

/// <summary>
/// Simple slime attack that damages player on touch
/// Add this to your slime GameObjects
/// </summary>
public class SlimeAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float damage = 10f;
    public float attackCooldown = 2f;

    [Header("References")]
    public Animator slimeAnimator;
    public string attackTrigger = "Attack";

    private bool canAttack = true;
    private bool playerInRange = false;
    private GameObject currentPlayer;

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"Slime trigger entered by: {other.name}");
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            currentPlayer = other.gameObject;
            //    Debug.Log("Player entered slime attack range!");

            if (canAttack)
            {
                AttackPlayer();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canAttack && !playerInRange)
        {
            playerInRange = true;
            currentPlayer = other.gameObject;
            AttackPlayer();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            currentPlayer = null;
            // Debug.Log("Player left slime attack range");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Slime collision with: {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            currentPlayer = collision.gameObject;
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        if (currentPlayer == null)
            return;

        canAttack = false;
        // Debug.Log("Slime attacking player!");

        // Play attack animation if available
        if (slimeAnimator != null && !string.IsNullOrEmpty(attackTrigger))
        {
            slimeAnimator.SetTrigger(attackTrigger);
            //   Debug.Log("Playing slime attack animation");
        }
        else
        {
            // If no animation, damage immediately
            DamagePlayer();
        }

        // Start cooldown
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    /// <summary>
    /// Call this from animation event, or it will be called automatically if no animation
    /// </summary>
    public void DamagePlayer()
    {
        if (currentPlayer != null)
        {
            Health playerHealth = currentPlayer.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                //       Debug.Log($"Slime dealt {damage} damage to player!");
            }
            else
            {
                //       Debug.LogWarning("Player has no Health component!");
            }
        }
    }

    void ResetAttack()
    {
        canAttack = true;
        //  Debug.Log("Slime can attack again");
    }

    void OnDrawGizmosSelected()
    {
        // Draw attack trigger area if it exists
        Collider col = GetComponent<Collider>();
        if (col != null && col.isTrigger)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, col.bounds.size);
        }
    }
}
