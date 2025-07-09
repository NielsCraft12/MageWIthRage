using System.Collections;
using UnityEngine;

public class newBreakableWall : MonoBehaviour
{
    [Header("Break Settings")]
    public GameObject breakEffect; // Optional particle effect
    public AudioClip breakSound; // Optional break sound

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Break()
    {
        // if (LevelManager.instance.abilitiesUnlocked < 3 &&)
        // {
        //     return;
        // }
        // Play break effect
        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, transform.rotation);
        }

        // Play break sound
        if (breakSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(breakSound);
        }

        // Destroy the wall
        Destroy(gameObject);
    }
}
