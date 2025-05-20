using UnityEngine;
using UnityEngine.InputSystem;

public class Attack2 : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    PlayerControlls playerControlls;

    [SerializeField]
    Animator animator;

    Health enemyHealth;

    private void Awake()
    {
        playerControlls = new PlayerControlls();
    }

    private void OnEnable()
    {
        playerControlls.Player.Attack.performed -= AttackEnemy; // Unsubscribe to prevent duplicates
        playerControlls.Player.Attack.performed += AttackEnemy; // Subscribe to input action
        playerControlls.Enable();
        //  Debug.Log("Input action subscribed and enabled");
    }

    private void OnDisable()
    {
        playerControlls.Player.Attack.performed -= AttackEnemy; // Unsubscribe to avoid memory leaks
        playerControlls.Disable();
        //    Debug.Log("Input action unsubscribed and disabled");
    }

    public void AttackEnemy(InputAction.CallbackContext context)
    {
        // Debug.Log("AttackEnemy called");
        // Play attack animation
        animator.SetTrigger("bonk");
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
    }
}
