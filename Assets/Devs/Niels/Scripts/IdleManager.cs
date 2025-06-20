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

    [SerializeField]
    Attack2 attack2;

    bool isColliderEnabled = false;

    void Start()
    {
        // Ensure attack collider starts disabled to prevent accidental enemy deaths
        attack2.enabled = false;
        wandCollider.enabled = false;
        isColliderEnabled = false;
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

        // Automatically disable attack collider when not attacking to prevent accidental damage
        if (
            isColliderEnabled
            && !playerActionsInput.BonkPressed
            && !playerActionsInput.BonkLvl1Pressed
        )
        {
            attack2.enabled = false;
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
        attack2.enabled = !isColliderEnabled;
        wandCollider.enabled = !isColliderEnabled;
        isColliderEnabled = wandCollider.enabled;
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
}
