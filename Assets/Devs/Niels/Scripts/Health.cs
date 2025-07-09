using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Health Regeneration")]
    [SerializeField]
    private float healthRegenRate = 5f; // Health points regenerated per second

    [SerializeField]
    private float regenDelay = 3f; // Delay before regeneration starts after taking damage

    [SerializeField]
    private bool canRegenerateHealth = true; // Toggle for health regeneration

    private float timeSinceLastDamage = 0f; // Timer to track time since last damage
    private Coroutine regenCoroutine; // Reference to the regeneration coroutine

    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private List<Renderer> renderers = new List<Renderer>();
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    [SerializeField]
    private Slider healthSlider; // Reference to the UI Slider for health display

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
        timeSinceLastDamage = 0f; // Reset the damage timer

        if (currentHealth <= 0)
        {
            Die(); // Call the Die
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth; // Update health slider
        }

        // Stop regeneration if taking damage
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }
    }

    private void Update()
    {
        // Track time since last damage for regeneration delay
        if (canRegenerateHealth && currentHealth < maxHealth)
        {
            timeSinceLastDamage += Time.deltaTime;

            // Start regeneration if enough time has passed since last damage
            if (timeSinceLastDamage >= regenDelay && regenCoroutine == null)
            {
                regenCoroutine = StartCoroutine(RegenerateHealth());
            }
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (currentHealth < maxHealth && timeSinceLastDamage >= regenDelay)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't exceed max

            // Update health slider if available
            if (healthSlider != null)
            {
                healthSlider.value = currentHealth / maxHealth;
            }

            yield return null; // Wait for next frame
        }

        regenCoroutine = null; // Reset coroutine reference
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
