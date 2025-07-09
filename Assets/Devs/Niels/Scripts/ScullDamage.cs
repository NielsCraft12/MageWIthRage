using UnityEngine;

public class ScullDamage : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log($"Player hit by Scull, took {damageAmount} damage.");
            }
            else
            {
                Debug.LogWarning("Player does not have a Health component.");
            }
        }
    }
}
