using UnityEngine;

public class SimplePlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("References")]
    public Animator animator;
    public string deathAnimationTrigger = "Death";

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Player healed {amount}. Current health: {currentHealth}");
    }

    void Die()
    {
        Debug.Log("Player died!");

        if (animator != null)
        {
            animator.SetTrigger(deathAnimationTrigger);
        }

        // Disable player controls or restart level here
        // GetComponent<PlayerActionsnput>().enabled = false;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}
