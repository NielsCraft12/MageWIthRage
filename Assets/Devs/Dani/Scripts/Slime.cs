using System.Collections;
using UnityEditor.ShortcutManagement;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;

public class Slime : Enemy
{
    [Header("Dependencies")]
    [SerializeField]
    private Rigidbody _rb;

    [SerializeField]
    private Transform _player;

    [SerializeField]
    private ParticleSystem _particleSystem;

    [Header("Settings")]
    [SerializeField]
    private float _jumpForce = 5f;

    [SerializeField]
    private float _jumpHeight = 2f;

    [SerializeField]
    private float _jumpCooldown = 2f;

    [SerializeField]
    private float _idleDistance = 1f;

    [Tooltip("Multiplier of velocity that increases the particle system's startspeed")]
    [Range(0, 2)]
    [SerializeField]
    private float _PSVelocityScale = 1f;

    [Tooltip("Minimal speed required to play the particle system")]
    [Range(0, 5)]
    [SerializeField]
    private float _minPSVelocity = 1f;

    [Range(0, 5)]
    [SerializeField]
    private float _rotateSpeed = 1f;
    private Vector3 _startPos;
    Vector3 lookDir;
    private bool _canjump = true;
    private Coroutine _jumpCoroutine;

    private Coroutine _damageCoroutine;

    [Header("Events")]
    public UnityEvent onJump;
    public UnityEvent onLand;

    void Awake()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        if (lookDir != transform.eulerAngles)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDir),
                Time.deltaTime * _rotateSpeed
            );
    }

    void FixedUpdate()
    {
        switch (alertState)
        {
            case AlertState.Idle:
                return;
            case AlertState.ToPlayer:
                if (_canjump)
                    ToPlayer();
                return;
            case AlertState.ToOrigin:
                if (_canjump)
                    ToOrigin();
                break;
        }

        if (Vector3.Distance(transform.position, _startPos) < _idleDistance)
            alertState = AlertState.Idle;
    }

    #region Jumping
    void ToOrigin()
    {
        _canjump = false;
        _rb.AddForce(
            (Vector3.up * _jumpHeight) + (_startPos - transform.position).normalized * _jumpForce,
            ForceMode.Impulse
        );
        _jumpCoroutine = StartCoroutine(JumpCooldown());
        DirectionToTarget(_startPos);
        onJump.Invoke();
    }

    void ToPlayer()
    {
        _canjump = false;
        _rb.AddForce(
            (Vector3.up * _jumpHeight)
                + (_player.position - transform.position).normalized * _jumpForce,
            ForceMode.Impulse
        );
        _jumpCoroutine = StartCoroutine(JumpCooldown());
        DirectionToTarget(_player.position);
        onJump.Invoke();
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(_jumpCooldown);
        _canjump = true;
        StopCoroutine(_jumpCoroutine);
    }
    #endregion

    void DirectionToTarget(Vector3 target)
    {
        lookDir = target - transform.position;
        lookDir.y = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        onLand.Invoke();

        if (_rb.linearVelocity.magnitude >= _minPSVelocity)
        {
            _particleSystem.Play();
            var main = _particleSystem.main;
            main.startSpeed = _rb.linearVelocity.magnitude * _PSVelocityScale;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (_damageCoroutine == null)
            {
                Debug.Log("Player hit by slime");
                _damageCoroutine = StartCoroutine(Damage(collision));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
        }
    }

    private IEnumerator Damage(Collision collision)
    {
        while (true)
        {
            Debug.Log("testghuwieal");
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Deal 10 damage (adjust as needed)
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
