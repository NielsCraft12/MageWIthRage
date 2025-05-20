using UnityEngine;

public class GhostHead : MonoBehaviour
{
    [Header("Dependencies")]
    public Rigidbody rb;
    [SerializeField] private Transform _ghost;

    [Header("Settings")]
    [SerializeField] private float _returnSpeed = 5f;

    private bool _hitPlayer;

    void OnDisable()
    {
        rb.linearVelocity = Vector3.zero;
        _hitPlayer = false;
    }

    private void FixedUpdate()
    {
        rb.AddForce((_ghost.position - transform.position) * _returnSpeed, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !_hitPlayer)
        {
            Debug.Log("Ghost head hit the player!");
            _hitPlayer = true;
        }
    }
}
