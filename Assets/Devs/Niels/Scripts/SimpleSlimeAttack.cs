using System.Collections;
using UnityEngine;

public class SimpleSlimeAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDamage = 10f;
    public float attackCooldown = 2f;

    [Header("References")]
    public Animator animator;
    public string attackAnimationTrigger = "Attack";

    private bool canAttack = true;
    private Health playerHealth;

    void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player") && canAttack)
        {
            playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                StartAttack();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Alternative: Check if the player collided with the slime
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                StartAttack();
            }
        }
    }

    void StartAttack()
    {
        canAttack = false;

        // Play attack animation
        if (animator != null)
        {
            animator.SetTrigger(attackAnimationTrigger);
        }

        // Start cooldown
        StartCoroutine(AttackCooldown());
    }

    // This method can be called by animation event during the attack animation
    public void DealDamageToPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log($"Slime attacked player for {attackDamage} damage!");
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        playerHealth = null; // Clear reference
    }
}
