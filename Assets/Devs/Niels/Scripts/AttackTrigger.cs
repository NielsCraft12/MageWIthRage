using UnityEngine;
using UnityEngine.InputSystem;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    PlayerControlls playerControlls;

    [SerializeField]
    Animator animator;

    private void Awake()
    {
        playerControlls = new PlayerControlls();
        playerControlls.Player.Attack.performed += ctx => AttackEnemy();
    }

    private void OnDisable()
    {
        playerControlls.Disable();
    }

    void OnEnable()
    {
        playerControlls.Enable();
    }

    void OnTriggerEnter(Collider other)
    {
        enemy = other.gameObject;
    }

    public void AttackEnemy()
    {
        if (!enemy.GetComponent<Health>() || enemy == null)
        {
            return;
        }
        // Example attack logic: reduce enemy health
        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(10); // Deal 10 damage (adjust as needed)
        }
        // Play attack animation
        animator.SetTrigger("bonk");
    }
}
