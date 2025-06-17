using System.Linq;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    public Animator animator;

    [SerializeField]
    private float localmotionBlendSpeed = 0.02f;
    PlayerState playerState;
    PlayerCotroller playerController;

    private bool IdleOnce = false;

    private PlayerLocalmotoininput playerLocalMotoinInput;
    private PlayerActionsnput playerActionsInput;

    private static int inputXHash = Animator.StringToHash("InputX");
    private static int inputYHash = Animator.StringToHash("InputY");
    private static int inputMagnetugeHash = Animator.StringToHash("InputMagnitude");

    [HideInInspector]
    public int idleHash = Animator.StringToHash("Idle");
    private static int isGroundedHash = Animator.StringToHash("IsGrounded");
    private static int isJumpingHash = Animator.StringToHash("IsJumping");
    private static int isFallingHash = Animator.StringToHash("IsFalling");
    private static int isBonkingHashLvl1 = Animator.StringToHash("IsBongingLvl1");
    private static int isBonkingHash = Animator.StringToHash("IsBonking");
    private static int isPlayingActionHash = Animator.StringToHash("IsplayingAction");

    private int[] actionHashes;

    private Vector3 currentBlendInput = Vector3.zero;

    private void Awake()
    {
        playerLocalMotoinInput = GetComponent<PlayerLocalmotoininput>();
        playerState = GetComponent<PlayerState>();
        playerController = GetComponent<PlayerCotroller>();
        playerActionsInput = GetComponent<PlayerActionsnput>();
        actionHashes = new int[] { isBonkingHashLvl1, isBonkingHash };
    }

    private void Update()
    {
        UpdateanimationState();
    }

    private void UpdateanimationState()
    {
        bool isIdling = playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
        bool isRunning = playerState.CurrentPlayerMovementState == PlayerMovementState.Running;
        bool isSprinting = playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
        bool isJumping = playerState.CurrentPlayerMovementState == PlayerMovementState.Jumping;
        bool isFalling = playerState.CurrentPlayerMovementState == PlayerMovementState.Falling;
        bool isGrounded = playerState.InGroundState();
        bool isPlayingAction = actionHashes.Any(hash => animator.GetBool(hash));

        Vector2 inputTarget = isSprinting
            ? playerLocalMotoinInput.MovementInput * 1.5f
            : playerLocalMotoinInput.MovementInput;

        currentBlendInput = Vector3.Lerp(
            currentBlendInput,
            inputTarget,
            localmotionBlendSpeed * Time.deltaTime
        );

        animator.SetBool(isGroundedHash, isGrounded);
        animator.SetBool(isJumpingHash, isJumping);
        animator.SetBool(isFallingHash, isFalling);
        animator.SetBool(isBonkingHash, playerActionsInput.BonkPressed);
        animator.SetBool(isBonkingHashLvl1, playerActionsInput.BonkLvl1Pressed);
        animator.SetBool(isPlayingActionHash, isPlayingAction);

        animator.SetFloat(inputXHash, currentBlendInput.x);
        animator.SetFloat(inputYHash, currentBlendInput.y);
        animator.SetFloat(inputMagnetugeHash, currentBlendInput.magnitude);
    }
}
