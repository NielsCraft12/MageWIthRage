using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private float rotationSpeed = 100f; // Adjust rotation speed as needed

    [Header("Camera Rotation Settings")]
    [SerializeField]
    private float camRotationSpeed = 100f; // Adjust rotation speed as needed

    [SerializeField]
    private float camBackRotationSpeed = 100f; // Adjust rotation speed as needed

    [SerializeField]
    private float TimeCamToRotateBack = 1f;
    private float timer;

    [Header("ARTIST DONT TOUCH THIS")]
    [SerializeField]
    private Transform cameraTransform; // Reference to the camera

    [SerializeField]
    private GameObject camLookPoint;
    private Rigidbody rb;
    private Vector2 move;
    private Vector2 rotate;
    private bool isJumping;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        rotate = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            isJumping = true;
        }
    }

    private void Update()
    {
        if (move.sqrMagnitude > 0.01f) // Check if the movement vector is significant
        {
            Vector3 targetDirection = new Vector3(move.x, 0, move.y).normalized;

            // Adjust movement direction based on camera orientation
            targetDirection = cameraTransform.TransformDirection(targetDirection);
            targetDirection.y = 0; // Keep movement on the horizontal plane

            Vector3 movement = targetDirection * speed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z); // Maintain vertical velocity

            // Rotate the player to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            ); // Smooth rotation
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); // Stop horizontal movement
        }

        // Rotate the camLookPoint left and right based on the rotate input
        if (camLookPoint != null)
        {
            camLookPoint.transform.Rotate(0, rotate.x * camRotationSpeed * Time.deltaTime, 0);

            // Reset camLookPoint rotation if it exceeds a certain angle
            float yRotation = camLookPoint.transform.localEulerAngles.y;
            if (yRotation > 180)
            {
                yRotation -= 360; // Normalize angle to [-180, 180]
            }

            if (Mathf.Abs(yRotation) > 5)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.y, 0);
                    camLookPoint.transform.rotation = Quaternion.Lerp(
                        camLookPoint.transform.rotation,
                        targetRotation,
                        Time.deltaTime * camBackRotationSpeed
                    );
                }
            }
            else
            {
                timer = TimeCamToRotateBack; // Only reset timer when within range
            }
        }

        Debug.Log(rotate);
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }
    }

    private bool IsGrounded()
    {
        // Check if the player is grounded using a raycast
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
