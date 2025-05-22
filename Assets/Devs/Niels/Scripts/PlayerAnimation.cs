using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float localmotionBlendSpeed = 0.02f;
    PlayerState playerState;
    PlayerCotroller playerController;

    [SerializeField]
    private bool IdleOnce = false;

    private PlayerLocalmotoininput playerLocalMotoinInput;

    private static int inputXHash = Animator.StringToHash("InputX");
    private static int inputYHash = Animator.StringToHash("InputY");
    private static int inputMagnetugeHash = Animator.StringToHash("InputMagnitude");

    private Vector3 currentBlendInput = Vector3.zero;

    private void Awake()
    {
        playerLocalMotoinInput = GetComponent<PlayerLocalmotoininput>();
        playerState = GetComponent<PlayerState>();
        playerController = GetComponent<PlayerCotroller>();
    }

    private void Update()
    {
        UpdateanimationState();
    }

    private void UpdateanimationState()
    {
        // if (currentBlendInput.magnitude < playerController.movingThreshold + 0.07f)
        // {
        //     currentBlendInput = Vector3.zero;
        //     if (!IdleOnce)
        //     {
        //         animator.Play("Blend Tree", 0, 0f);
        //         IdleOnce = true;
        //     }
        // }
        // else if (currentBlendInput.magnitude > playerController.movingThreshold + 0.07f)
        // {
        //     if (IdleOnce)
        //     {
        //         IdleOnce = false;
        //     }
        // }

        bool isSprinting = playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;

        Vector2 inputTarget = isSprinting
            ? playerLocalMotoinInput.MovementInput * 1.5f
            : playerLocalMotoinInput.MovementInput;

        currentBlendInput = Vector3.Lerp(
            currentBlendInput,
            inputTarget,
            localmotionBlendSpeed * Time.deltaTime
        );

        animator.SetFloat(inputXHash, currentBlendInput.x);
        animator.SetFloat(inputYHash, currentBlendInput.y);
        animator.SetFloat(inputMagnetugeHash, currentBlendInput.magnitude);
    }
}
