using UnityEngine;

/// <summary>
/// A passive enemy that can be damaged by the player but does not attack back.
/// This is specifically designed for ghost enemies that should only receive damage.
/// </summary>
public class GhostDamage : MonoBehaviour
{
    [Header("Ghost Health Settings")]
    [SerializeField]
    private float maxHealth = 50f;

    [SerializeField]
    private float currentHealth;

    [Header("Visual Feedback")]
    [SerializeField]
    private Color flashColor = Color.red;

    [SerializeField]
    private float flashDuration = 0.1f;

    [Header("Death Settings")]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private string deathAnimationTrigger = "death";

    [SerializeField]
    private float destroyDelay = 2f; // Time before destroying the ghost after death

    private Renderer[] renderers;
    private Color[] originalColors;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        // Get all renderers for visual feedback
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        // Store original colors
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                originalColors[i] = renderers[i].material.color;
            }
        }
    }

    /// <summary>
    /// Called by the player's attack system to damage the ghost
    /// </summary>
    /// <param name="damage">Amount of damage to take</param>
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        // Flash effect
        StartCoroutine(FlashEffect());

        Debug.Log($"Ghost took {damage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log("Ghost died!");

        // Play death animation if animator is available
        if (animator != null)
        {
            animator.SetTrigger(deathAnimationTrigger);
        }

        // Disable collider so it can't be hit again
        Collider ghostCollider = GetComponent<Collider>();
        if (ghostCollider != null)
        {
            ghostCollider.enabled = false;
        }

        // Destroy the ghost after a delay
        Destroy(gameObject, destroyDelay);
    }

    private System.Collections.IEnumerator FlashEffect()
    {
        // Change to flash color
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                renderers[i].material.color = flashColor;
            }
        }

        yield return new WaitForSeconds(flashDuration);

        // Return to original color
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                renderers[i].material.color = originalColors[i];
            }
        }
    }

    // Public getters for debugging
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;
}
