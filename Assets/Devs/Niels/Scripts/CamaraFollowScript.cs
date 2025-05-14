using UnityEngine;

public class CamaraFollowScript : MonoBehaviour
{
    [Header("Camera Follow Settings")]
    [SerializeField]
    private Transform playerTransform; // Reference to the player's transform

    [SerializeField]
    private float distance = 10f; // Distance from the player

    [SerializeField]
    private float height = 5f; // Height above the player

    [SerializeField]
    private float followSpeed = 2f; // Speed of the camera following the player

    [SerializeField]
    private float lookSpeed = 5f; // Speed of the camera rotation

    private void FixedUpdate()
    {
        Vector3 offset = playerTransform.forward * (-1) * distance + playerTransform.up * height; // Calculate the offset position
        Vector3 desiredPosition = playerTransform.position + offset; // Calculate the desired position of the camera
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        ); // Smoothly interpolate to the desired position
        transform.position = smoothedPosition; // Update the camera position

        Quaternion desiredRotation = Quaternion.LookRotation(
            playerTransform.position - transform.position
        );
        Quaternion smoothedRotation = Quaternion.Lerp(
            transform.rotation,
            desiredRotation,
            lookSpeed * Time.deltaTime
        ); // Calculate the rotation to look at the player

        transform.rotation = smoothedRotation; // Update the camera rotation
    }
}
