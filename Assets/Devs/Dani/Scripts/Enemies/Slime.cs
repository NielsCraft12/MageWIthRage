using System.Collections;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Events;

public class Slime : Enemy
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _player;
    [SerializeField] private ParticleSystem _ps;

    [Header("Settings")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _jumpCooldown = 2f;
    [SerializeField] private float _idleDistance = 1f;

    [Tooltip("Multiplier of velocity that increases the particle system's startspeed")]
    [Range(0, 2)][SerializeField] private float _PSVelocityScale = 1f;
    [Tooltip("Minimal speed required to play the particle system")]
    [Range(0, 5)][SerializeField] private float _minPSVelocity = 1f;
    [Range(0, 5)][SerializeField] private float _rotateSpeed = 1f;
    private Vector3 _startPos;
    Vector3 lookDir;
    private bool _canJump = true;
    private bool _canRotate = false;
    private bool _controlsSelf = true; // Whether the slime is in control if it's own movement and not by the physics system
    private int _controlFrames; // Amount of frames the slime should be in control
    [SerializeField] private int _controlFramesRequired = 10; // Amount of frames required to be in control
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
        if (_canRotate && lookDir != transform.eulerAngles)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDir),
                Time.deltaTime * _rotateSpeed
            );
    }

    void FixedUpdate()
    {
        if (!_controlsSelf && _rb.linearVelocity.magnitude < 0.1f)
        {
            _controlFrames++;
            if (_controlFrames >= _controlFramesRequired)
            {
                _controlsSelf = true;
                _controlFrames = 0;
            }
            return;
        }
        else if (!_controlsSelf)
        {
            _controlFrames = 0;
            return;
        }

        // _rb.constraints = RigidbodyConstraints.None;

        switch (alertState)
        {
            case AlertState.Idle:
                return;
            case AlertState.ToPlayer:
                if (_canJump)
                    ToPlayer();
                return;
            case AlertState.ToOrigin:
                if (_canJump)
                    ToOrigin();
                break;
        }

        if (Vector3.Distance(transform.position, _startPos) < _idleDistance)
            alertState = AlertState.Idle;
    }

    #region Jumping
    void ToOrigin()
    {
        _canJump = false;
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
        _canJump = false;
        onJump.Invoke();
        StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        DirectionToTarget(_player.position);
        _canRotate = true;
        yield return new WaitForSeconds(.9f);
        _rb.AddForce(
            (Vector3.up * _jumpHeight) + lookDir.normalized * _jumpForce,
            ForceMode.Impulse
        );
        _jumpCoroutine = StartCoroutine(JumpCooldown());
    }
    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(_jumpCooldown);
        _canJump = true;
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
        _canRotate = false;
        onLand.Invoke();

        if (_rb.linearVelocity.magnitude >= _minPSVelocity)
        {
            _ps.Play();
            var main = _ps.main;
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

    private void ToControl()
    {
        _controlsSelf = true;
        _controlFrames = 0;
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void LoseControl()
    {
        _controlsSelf = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }
}
