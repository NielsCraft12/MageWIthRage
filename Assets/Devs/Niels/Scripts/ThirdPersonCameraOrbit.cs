using UnityEngine;

public class ThirdPersonCameraOrbit : MonoBehaviour
{
    public Transform target; // The player
    public Transform cameraTransform; // The actual camera
    public Vector2 sensitivity = new Vector2(2f, 1.5f);
    public Vector2 clampAngles = new Vector2(-40f, 80f);
    public float distance = 6f;

    private Vector2 lookInput;
    private float yaw;
    private float pitch;

    private PlayerControlls inputActions;

    void Awake()
    {
        inputActions = new PlayerControlls();
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
    }

    void OnEnable() => inputActions.Enable();

    void OnDisable() => inputActions.Disable();

    void LateUpdate()
    {
        yaw += lookInput.x * sensitivity.x;
        pitch -= lookInput.y * sensitivity.y;
        pitch = Mathf.Clamp(pitch, clampAngles.x, clampAngles.y);

        // Rotate camera rig
        transform.position = target.position;
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        // Move the camera back
        cameraTransform.position = transform.position - transform.forward * distance;
        cameraTransform.LookAt(target);
    }
}
