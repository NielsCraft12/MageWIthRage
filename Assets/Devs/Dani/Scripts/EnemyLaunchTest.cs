using UnityEngine;

public class EnemyLaunchTest : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Slime>() != null)
        {
            // If the colliding object is a Slime, launch it away
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                collision.gameObject.GetComponent<Slime>().LoseControl();
                Vector3 launchDirection = (collision.transform.position - transform.position).normalized;
                rb.AddForce(launchDirection * 10f, ForceMode.Impulse);
            }
        }
    }
}
