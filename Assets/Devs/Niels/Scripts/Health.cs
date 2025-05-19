using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100f; // Maximum health of the player

    [SerializeField]
    protected float currentHealth; // Current health of the player

    [Header("Damage Settings")]
    [SerializeField]
    protected float damageAmount = 10f; // Amount of damage taken from enemy collision

    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health to maximum health
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Reduce current health by damage amount
        if (currentHealth <= 0)
        {
            Die(); // Call the Die
        }
    }

    private void Die()
    {
        // Handle player death (e.g., respawn, game over, etc.)
        Debug.Log(transform.gameObject.name + " has died.");
        gameObject.SetActive(false); // Deactivate the player object
        // You can add more logic here, such as respawning the player or ending the game.
    }
}
