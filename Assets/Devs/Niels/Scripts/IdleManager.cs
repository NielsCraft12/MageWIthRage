using UnityEngine;

public class IdleManager : MonoBehaviour
{
    [SerializeField]
    PlayerAnimation playerAnimation;

    [SerializeField]
    PlayerState playerState;

    [SerializeField]
    PlayerActionsnput playerActionsInput;

    [SerializeField]
    float waittime = 0.85f;

    float idleIndex = 0f;

    bool isSitting = true;

    [SerializeField]
    BoxCollider wandCollider;

    [SerializeField]
    GameObject backWand;

    [SerializeField]
    GameObject handWandCollider;

    bool isColliderEnabled = false;

    void Start()
    {
        // IdleManager initialization - Attack2 manages its own state
    }

    void Update()
    {
        if (playerState.CurrentPlayerMovementState == PlayerMovementState.Idling && isSitting)
        {
            playerAnimation.animator.Play("Blend Tree", 0, 0f);
            isSitting = false;
        }
        if (playerState.CurrentPlayerMovementState != PlayerMovementState.Idling)
        {
            isSitting = true;
        }

        if (
            playerState.CurrentPlayerMovementState == PlayerMovementState.Running
            || playerState.CurrentPlayerMovementState == PlayerMovementState.Walking
            || playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting
        )
        {
            backWand.SetActive(false);
            handWandCollider.SetActive(true);
        }

        // Automatically disable old wand collider when not attacking to prevent accidental damage
        // Keep Attack2 component enabled - it manages its own timing
        if (
            isColliderEnabled
            && !playerActionsInput.BonkPressed
            && !playerActionsInput.BonkLvl1Pressed
        )
        {
            // Only disable the old wand collider, NOT the Attack2 component
            if (wandCollider != null)
                wandCollider.enabled = false;
            isColliderEnabled = false;
        }
    }

    public void SetAttackPressedFalse()
    {
        if (playerActionsInput.attackActive == 0)
        {
            playerActionsInput.BonkLvl1Pressed = false;
        }
        else if (playerActionsInput.attackActive == 1)
        {
            playerActionsInput.BonkPressed = false;
        }
    }

    public void ToggleCollider()
    {
        // Only toggle the old wand collider system, keep Attack2 always enabled
        if (wandCollider != null)
        {
            wandCollider.enabled = !isColliderEnabled;
            isColliderEnabled = wandCollider.enabled;
        }
    }

    public void ToggleBackwand()
    {
        if (playerState.CurrentPlayerMovementState == PlayerMovementState.Idling)
        {
            wandCollider.enabled = false;
            backWand.SetActive(!backWand.activeSelf);
            handWandCollider.SetActive(!handWandCollider.activeSelf);
        }
    }

    /// <summary>
    /// Called by Animation Event to trigger attack damage at precise timing
    /// Add this method name to your attack animation events
    /// </summary>
    public void TriggerAttackDamage()
    {
        // Use the integrated attack system in PlayerActionsInput
        if (playerActionsInput != null)
        {
            playerActionsInput.TriggerAttackDamage();
        }
    }

    /// <summary>
    /// Called by Animation Event when attack animation ends
    /// Add this method name to your attack animation events
    /// </summary>
    public void EndAttack()
    {
        // Use the integrated attack system in PlayerActionsInput
        if (playerActionsInput != null)
        {
            playerActionsInput.EndAttack();
        }
    }
}
