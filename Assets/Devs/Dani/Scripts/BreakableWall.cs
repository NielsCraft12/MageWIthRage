using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BreakableWall2 : MonoBehaviour
{
    [Header("Wall Health")]
    public float health = 100f;
    public float maxHealth = 100f;

    [Header("Effects")]
    public ParticleSystem breakEffect;
    public AudioSource breakSound;

    [Header("Events")]
    public UnityEvent onBreak;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(
            $"BreakableWall2 {name} BEFORE damage - Health: {health}/{maxHealth}, Incoming damage: {damage}"
        );

        health -= damage;

        Debug.Log($"BreakableWall2 {name} AFTER damage - Health: {health}/{maxHealth}");

        if (health <= 0)
        {
            Debug.Log($"BreakableWall2 {name} health <= 0, calling Break()!");
            Break();
        }
        else
        {
            Debug.Log($"BreakableWall2 {name} still has {health} health remaining");
        }
    }

    public void Break()
    {
        Debug.Log($"BreakableWall2 {name} is breaking!");

        // Play effects
        if (breakEffect != null)
            breakEffect.Play();

        if (breakSound != null)
            breakSound.Play();

        // Trigger events
        onBreak.Invoke();

        // Start break coroutine
        StartCoroutine(BreakWall());
    }

    private IEnumerator BreakWall()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}
