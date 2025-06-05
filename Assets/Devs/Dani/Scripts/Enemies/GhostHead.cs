using Unity.Collections;
using UnityEngine;

public class GhostHead : MonoBehaviour
{
    [Header("Dependencies")]
    public Rigidbody rb;
    [SerializeField] private Transform _ghost;

    [Header("Settings")]
    [SerializeField] private float _returnSpeed = 25f;
    [ReadOnly][SerializeField] private float _returnMult = 0f;
    [Tooltip("The return speed multiplier increase each FixedUpdate tick. Return multiplier caps at 1x")][SerializeField] private float _returnMultOnFixedUpdate = 0.025f;

    private bool _hitPlayer;

    void OnDisable()
    {
        rb.linearVelocity = Vector3.zero;
        _hitPlayer = false;
        _returnMult = 0f;
    }

    private void FixedUpdate()
    {
        if (_returnMult < 1f)
        {
            _returnMult += _returnMultOnFixedUpdate;
            if (_returnMult > 1f) _returnMult = 1f;
        }

        rb.AddForce((_ghost.position - transform.position).normalized * _returnSpeed * _returnMult, ForceMode.Acceleration);
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
