using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private Animator animator; // Reference to the Animator component

    [SerializeField]
    private string deathAnimationTrigger = "death"; // Name of the idle animation

    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100f; // Maximum health of the player

    [SerializeField]
    protected float currentHealth; // Current health of the player

    [Header("Damage Settings")]
    [SerializeField]
    protected float damageAmount = 10f; // Amount of damage taken from enemy collision

    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private List<Renderer> renderers = new List<Renderer>();
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    // Public getters for debugging
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health to maximum health

        // Get all Renderer components in this object and its children
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        // Store original colors
        foreach (Renderer rend in renderers)
        {
            if (rend.material.HasProperty("_Color"))
                originalColors[rend] = rend.material.color;
        }
    }

    public void TakeDamage(float damage)
    {
        Flash();
        currentHealth -= damage; // Reduce current health by damage amount
        if (currentHealth <= 0)
        {
            Die(); // Call the Die
        }
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        foreach (Renderer rend in renderers)
        {
            if (rend.material.HasProperty("_Color"))
                rend.material.color = flashColor;
        }

        yield return new WaitForSeconds(flashDuration);

        foreach (Renderer rend in renderers)
        {
            if (rend.material.HasProperty("_Color"))
                rend.material.color = originalColors[rend];
        }
    }

    private void Die()
    {
        // Handle player death (e.g., respawn, game over, etc.)
        if (gameObject.GetComponent<Slime>() != null)
        {
            gameObject.GetComponent<Slime>().enabled = false; // Disable the player movement script
        }
        if (animator != null)
        {
            animator.SetTrigger(deathAnimationTrigger); // Trigger the death animation
        }
        Debug.Log(transform.gameObject.name + " has died.");
        // gameObject.SetActive(false); // Deactivate the player object
        Destroy(gameObject, 2f); // Destroy the player object after 2 seconds
        // You can add more logic here, such as respawning the player or ending the game.
    }
}
