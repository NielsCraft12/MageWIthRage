using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Component reference
    private Rigidbody rb;

    [Header("Movement")]
    // Movement settings
    [SerializeField]
    private float maxSpeed; // movement speed

    [SerializeField]
    private float rotationSpeed = 5f; // Speed at which player rotates
    private Vector2 moveDirection; // Current movement input direction

    // Initialize physics properties
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearDamping = 0.5f; // Remove air resistance
        rb.mass = 1f; // Set consistent mass
        rb.angularDamping = 0.05f; // Minimal rotation resistance
        rb.useGravity = true; // Ensure gravity is on
    }

    private void Start() { }

    // Handle timers and visual effects
    void Update()
    {
        LookAt();
    }

    // Physics-based movement calculations
    private void FixedUpdate()
    {
        // Maintain the vertical velocity to avoid interference with jumping
        float yVelocity = rb.linearVelocity.y;

        // Transform the input direction to be relative to the player's rotation
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 relativeMoveDirection = (
            forward * moveDirection.y + right * moveDirection.x
        ).normalized;

        // Apply the calculated movement direction and speed
        rb.linearVelocity = new Vector3(
            relativeMoveDirection.x * maxSpeed,
            yVelocity,
            relativeMoveDirection.z * maxSpeed
        );
    }

    // Toggle pause state
    // public void Pause(InputAction.CallbackContext _context)
    // {
    //     SettingsSingleton.Instance.settings.m_IsPaused = !SettingsSingleton
    //         .Instance
    //         .settings
    //         .m_IsPaused;
    // }

    // Handle trap placement with cooldown


    // Process movement input
    public void OnMove(InputAction.CallbackContext _context)
    {
        moveDirection = _context.ReadValue<Vector2>();
    }

    // Handle cleaning action input


    // Rotate player to face movement direction
    private void LookAt()
    {
        Vector3 direction = rb.linearVelocity;
        direction.y = 0f;

        if (moveDirection.sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.rotation = Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
}
