using UnityEngine;

public class DeathOnTouch : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // PlayerDeath();
        }
    }
}
