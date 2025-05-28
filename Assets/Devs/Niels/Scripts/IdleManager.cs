using UnityEngine;

public class IdleManager : MonoBehaviour
{
    [SerializeField]
    private PlayerAnimation playerAnimation;

    [SerializeField]
    private PlayerState playerState;

    [SerializeField]
    private float waittime = 0.85f;

    private float idleIndex = 0f;

    private bool isSitting = true;

    private bool isAttackTriggerActive = false;

    [SerializeField]
    private BoxCollider attackTrigger;

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

    public void ToggleTrigger()
    {
        if (isAttackTriggerActive)
        {
            attackTrigger.enabled = false;
            isAttackTriggerActive = false;
        }
        else
        {
            attackTrigger.enabled = true;
            isAttackTriggerActive = true;
        }
    }
}
