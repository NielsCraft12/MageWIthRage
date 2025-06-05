using UnityEngine;

[DefaultExecutionOrder(-1)]
public class PlayerCotroller : MonoBehaviour
{
    #region Class Variables
    [Header("Components")]
    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private Camera playerCamera;

    [Header("Base Movement")]
    public float runAcceleration = 0.25f;
    public float runSpeed = 0.25f;
    public float sprintAcceleration = 0.5f;
    public float SprintSpeed = 7f;
    public float drag = 0.1f;
    public float inAirDrag = 0.1f;
    public float gravity = 25f;
    public float jumpSpeed = 1f;
    public float movingThreshold = 0.01f;

    [Header("Camara Settings")]
    public float lookSenseH = 0.1f;
    public float lookSenseV = 0.1f;
    public float lookLimitV = 89f;

    private PlayerLocalmotoininput playerLocalMotoinInput;
    private PlayerState playerState;
    Vector2 cameraRotation = Vector2.zero;
    private Vector2 playerRotation = Vector2.zero;

    private float vericleVelocity = 0f;
    #endregion
    #region Startup
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerLocalMotoinInput = GetComponent<PlayerLocalmotoininput>();
        playerState = GetComponent<PlayerState>();
    }
    #endregion
    #region Update Cheacks

    private void Update()
    {
        HandleLateralMeovment();
        HandleVirticleMeovment();
        UpdateMovementState();
    }

    private void UpdateMovementState()
    {
        bool isMovementInput = playerLocalMotoinInput.MovementInput != Vector2.zero;
        bool isMovingLaterally = IsMovingLaterally(); // order matters
        bool isSprinting = playerLocalMotoinInput.SprintToggleOn && isMovingLaterally;
        bool isGrounded = IsGrounded();

        PlayerMovementState lateralMovementState = isSprinting
            ? PlayerMovementState.Sprinting
            : isMovingLaterally || isMovementInput
                ? PlayerMovementState.Running
                : PlayerMovementState.Idling;

        playerState.setPlayerMovementState(lateralMovementState);

        if (!isGrounded && characterController.velocity.y > 0f)
        {
            playerState.setPlayerMovementState(PlayerMovementState.Jumping);
        }
        else if (!isGrounded && characterController.velocity.y < 0f)
        {
            playerState.setPlayerMovementState(PlayerMovementState.Falling);
        }
    }

    private void HandleVirticleMeovment()
    {
        bool isGrounded = playerState.InGroundState();

        if (isGrounded && vericleVelocity < 0f)
        {
            vericleVelocity = 0f;
        }

        vericleVelocity -= gravity * Time.deltaTime;

        if (playerLocalMotoinInput.JumpPressed && isGrounded)
        {
            vericleVelocity = Mathf.Sqrt(jumpSpeed * 3 * gravity);
        }
    }

    private void HandleLateralMeovment()
    {
        bool isSprinting = playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
        bool isGrounded = playerState.InGroundState();

        float lateralAcceleration = isSprinting ? sprintAcceleration : runAcceleration;
        float clampLateralMagnatude = isSprinting ? SprintSpeed : runSpeed;

        Vector3 camaraForwardXZ = new Vector3(
            playerCamera.transform.forward.x,
            0f,
            playerCamera.transform.forward.z
        ).normalized;
        Vector3 camaraRightXZ = new Vector3(
            playerCamera.transform.right.x,
            0f,
            playerCamera.transform.right.z
        ).normalized;

        Vector3 moveDirection =
            camaraRightXZ * playerLocalMotoinInput.MovementInput.x
            + camaraForwardXZ * playerLocalMotoinInput.MovementInput.y;

        Vector3 movementDelta = moveDirection * lateralAcceleration * Time.deltaTime;
        Vector3 newVelocity;

        // If grounded, set velocity directly to movement input (no sliding)
        if (isGrounded)
        {
            newVelocity = moveDirection * clampLateralMagnatude;
        }
        else
        {
            // In air, keep previous velocity and add movement delta (optional: can be more restrictive)
            newVelocity = characterController.velocity + movementDelta;
            newVelocity = Vector3.ClampMagnitude(
                new Vector3(newVelocity.x, 0f, newVelocity.z),
                clampLateralMagnatude
            );
        }

        newVelocity.y = vericleVelocity;
        characterController.Move(newVelocity * Time.deltaTime);
    }
    #endregion

    #region LateUpdate

    void LateUpdate()
    {
        cameraRotation.x += lookSenseH * playerLocalMotoinInput.LookInput.x;
        cameraRotation.y = Mathf.Clamp(
            cameraRotation.y - lookSenseV * playerLocalMotoinInput.LookInput.y,
            -lookLimitV,
            lookLimitV
        );
        playerRotation.x +=
            transform.eulerAngles.x + lookSenseH * playerLocalMotoinInput.LookInput.x;
        transform.rotation = Quaternion.Euler(0f, playerRotation.x, 0f);

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0f);
    }
    #endregion
    #region State Checks
    private bool IsMovingLaterally()
    {
        Vector3 lateralVelocity = new Vector3(
            characterController.velocity.x,
            0f,
            characterController.velocity.z
        );

        return lateralVelocity.magnitude > movingThreshold;
    }

    private bool IsGrounded()
    {
        return characterController.isGrounded;
    }

    #endregion
}
