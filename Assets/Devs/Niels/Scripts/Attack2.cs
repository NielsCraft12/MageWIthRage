using UnityEngine;
using UnityEngine.InputSystem;

public class Attack2 : MonoBehaviour
{
    // [SerializeField]
    // private GameObject enemy;

    // PlayerControls playerControls;

    // [SerializeField]
    // Animator animator;

    Health enemyHealth;

    // private void Awake()
    // {
    //     playerControls = new PlayerControls();
    // }

    // private void OnEnable()
    // {
    //     playerControls.Actions.Attack.performed -= AttackEnemy; // Unsubscribe to prevent duplicates
    //     playerControls.Actions.Attack.performed += AttackEnemy; // Subscribe to input action
    //     playerControls.Enable();
    //     //  Debug.Log("Input action subscribed and enabled");
    // }

    // private void OnDisable()
    // {
    //     playerControls.Actions.Attack.performed -= AttackEnemy; // Unsubscribe to avoid memory leaks
    //     playerControls.Disable();
    //     //    Debug.Log("Input action unsubscribed and disabled");
    // }

    public void AttackEnemy(InputAction.CallbackContext context)
    {
        // Debug.Log("AttackEnemy called");
        // Play attack animation
        //        animator.SetTrigger("bonk");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("Enemy hit by attack");
            enemyHealth = other.gameObject.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10); // Deal 10 damage (adjust as needed)
            }
        }

        if (other.gameObject.CompareTag("Breakable"))
        {
            Debug.Log("Breakable object hit by attack");
            BreakableWall breakable = other.gameObject.GetComponent<BreakableWall>();
            if (breakable != null)
            {
                breakable.Break(); // Call the Break method on the Breakable component
            }
        }
    }
}
