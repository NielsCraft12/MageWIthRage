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

    bool isColliderEnabled = false;

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
    }

    public void SetAttackPressedFalse()
    {
        playerActionsInput.AttackPressed = false;
    }

    public void ToggleCollider()
    {
        wandCollider.enabled = !wandCollider.enabled;
        isColliderEnabled = wandCollider.enabled;
    }
}
