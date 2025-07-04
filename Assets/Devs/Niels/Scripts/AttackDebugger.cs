using UnityEngine;

/// <summary>
/// Temporary debug script to help identify what's causing the slime to take damage
/// Add this to any GameObject to help debug the issue
/// </summary>
public class AttackDebugger : MonoBehaviour
{
    void Update()
    {
        // Press 'D' to debug player attack state
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerActionsnput playerInput = player.GetComponent<PlayerActionsnput>();
                if (playerInput != null)
                {
                    // Use reflection to check private fields
                    var canAttackField = typeof(PlayerActionsnput).GetField(
                        "canAttack",
                        System.Reflection.BindingFlags.NonPublic
                            | System.Reflection.BindingFlags.Instance
                    );
                    var isAttackingField = typeof(PlayerActionsnput).GetField(
                        "isAttacking",
                        System.Reflection.BindingFlags.NonPublic
                            | System.Reflection.BindingFlags.Instance
                    );

                    bool canAttack =
                        canAttackField != null ? (bool)canAttackField.GetValue(playerInput) : false;
                    bool isAttacking =
                        isAttackingField != null
                            ? (bool)isAttackingField.GetValue(playerInput)
                            : false;

                    Debug.Log($"PLAYER DEBUG - CanAttack: {canAttack}, IsAttacking: {isAttacking}");
                    Debug.Log(
                        $"BonkPressed: {playerInput.BonkPressed}, BonkLvl1Pressed: {playerInput.BonkLvl1Pressed}"
                    );
                }
            }
        }

        // Press 'H' to manually test slime health and damage state
        if (Input.GetKeyDown(KeyCode.H))
        {
            Slime[] slimes = Object.FindObjectsByType<Slime>(FindObjectsSortMode.None);
            Debug.Log($"Found {slimes.Length} slimes");

            foreach (Slime slime in slimes)
            {
                Health slimeHealth = slime.GetComponent<Health>();
                if (slimeHealth != null)
                {
                    Debug.Log(
                        $"Slime {slime.name} health: {slimeHealth.CurrentHealth}/{slimeHealth.MaxHealth}"
                    );
                }
                else
                {
                    Debug.Log($"Slime {slime.name} has no Health component");
                }
            }
        }

        // Press 'S' to stop all slime damage coroutines (emergency stop)
        if (Input.GetKeyDown(KeyCode.S))
        {
            Slime[] slimes = Object.FindObjectsByType<Slime>(FindObjectsSortMode.None);
            foreach (Slime slime in slimes)
            {
                slime.StopAllCoroutines();
                Debug.Log($"Stopped all coroutines for slime {slime.name}");
            }
            Debug.Log("EMERGENCY STOP: All slime damage coroutines stopped!");
        }

        // Press 'T' to test attack timing
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerActionsnput playerInput = player.GetComponent<PlayerActionsnput>();
                if (playerInput != null)
                {
                    Debug.Log("=== TIMING COMPARISON ===");
                    Debug.Log(
                        "Slime damage: 10 damage immediately, then every 2 seconds (max 5 hits)"
                    );
                    Debug.Log("Slime cooldown: 3 seconds between damage sessions");
                    Debug.Log("Player attack: Check your animation length and cooldown");
                    Debug.Log("Player attack range: " + playerInput.attackRange);
                    Debug.Log("Player attack damage: " + playerInput.attackDamage);
                }
            }
        }
    }
}
