using System.Collections;
using UnityEditor.ShortcutManagement;
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
    private ParticleSystem _ps;

    [Header("Settings")]
    [SerializeField]
    private float _jumpForce = 5f;

    [SerializeField]
    private float _jumpHeight = 2f;

    [SerializeField]
    private float _jumpCooldown = 2f;

    [SerializeField]
    private float _idleDistance = 1f;

    [Tooltip("Magnitude of the velocity required to gain a control frame")]
    [Range(0, 2)]
    [SerializeField]
    private float _controlFrameAmount = 1f;

    [Tooltip("Multiplier of velocity that increases the particle system's startspeed")]
    [Range(0, 2)]
    [SerializeField]
    private float _PSVelocityScale = 1f;

    [Tooltip("Minimal speed required to play the particle system")]
    [Range(0, 5)]
    [SerializeField]
    private float _minPSVelocity = 1f;

    [Tooltip("Time in seconds after gaining control before the slime can jump again")]
    [Range(0, 2)]
    [SerializeField]
    private float _jumpCooldownAfterControlGain = 1f;

    [Range(0, 5)]
    [SerializeField]
    private float _rotateSpeed = 1f;
    private Vector3 _startPos;
    private Vector3 lookDir;

    [SerializeField]
    private bool _canJump = true;

    [SerializeField]
    private bool _canRotate = false;

    [SerializeField]
    private bool _controlsSelf = true; // Whether the slime is in control if it's own movement and not by the physics system

    [SerializeField]
    private int _controlFrames; // Amount of frames the slime should be in control

    [SerializeField]
    private int _controlFramesRequired = 10; // Amount of frames required to be in control
    private Coroutine _jumpCoroutine;
    private Coroutine _jumpResetCoroutine;
    private Coroutine _damageCoroutine;
    private float _lastDamageTime = 0f;
    private float _damageCooldown = 3f; // 3 second cooldown between damage sessions

    [Header("Events")]
    public UnityEvent onJump;
    public UnityEvent onLand;

    void Awake()
    {
        _startPos = transform.position;
    }

    void OnEnable()
    {
        _canJump = true;
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
        if (!_controlsSelf && _rb.linearVelocity.magnitude < _controlFrameAmount)
        {
            _controlFrames++;
            if (_controlFrames >= _controlFramesRequired)
            {
                ToControl();
            }
            return;
        }
        else if (!_controlsSelf)
        {
            _controlFrames = 0;
            return;
        }

        switch (alertState)
        {
            case AlertState.Idle:
                break;
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
        else
            alertState = AlertState.ToOrigin;
    }

    #region Jumping
    void ToOrigin()
    {
        _canJump = false;
        DirectionToTarget(_startPos);
        onJump.Invoke();
        StartCoroutine(Jump());
    }

    void ToPlayer()
    {
        _canJump = false;
        DirectionToTarget(_player.position);
        onJump.Invoke();
        StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
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
            // Check if we're on cooldown
            if (Time.time - _lastDamageTime < _damageCooldown)
            {
                Debug.Log(
                    $"Slime damage on cooldown for {_damageCooldown - (Time.time - _lastDamageTime):F1} more seconds"
                );
                return;
            }

            if (_damageCoroutine == null)
            {
                Debug.Log("Player hit by slime - starting damage!");
                _lastDamageTime = Time.time;
                _damageCoroutine = StartCoroutine(Damage(collision));
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
                _damageCoroutine = null;
                Debug.Log("Player left slime collision - stopping damage");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
                _damageCoroutine = null;
                Debug.Log("Player left slime trigger - stopping damage");
            }
        }
    }

    private IEnumerator Damage(Collision collision)
    {
        GameObject player = collision.gameObject;
        int damageCount = 0;

        // Deal initial damage immediately on contact
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
            Debug.Log("Slime dealt initial 10 damage to player on contact!");
            damageCount++;
        }

        while (damageCount < 5 && player != null) // Reduced from 20 to 5 total damage ticks
        {
            // Wait longer between damage ticks - 2 seconds instead of 0.5
            yield return new WaitForSeconds(2.0f);

            // Double-check player is still valid and has the right tag
            if (player == null || !player.CompareTag("Player"))
            {
                Debug.Log("Player object became invalid - stopping damage");
                break;
            }

            playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Deal 10 damage (adjust as needed)
                Debug.Log($"Slime dealt 10 damage to player! (tick {damageCount + 1})");
                damageCount++;
            }
            else
            {
                Debug.LogWarning("Player has no Health component!");
                break;
            }
        }

        // Clean up when done
        _damageCoroutine = null;
        Debug.Log("Damage coroutine ended");
    }

    private void ToControl()
    {
        _controlsSelf = true;
        _controlFrames = 0;
        DirectionToTarget(_player.position);
        _canRotate = true;
        _jumpResetCoroutine = StartCoroutine(JumpAfterControl());
        _rb.constraints =
            RigidbodyConstraints.FreezeRotationX
            | RigidbodyConstraints.FreezeRotationY
            | RigidbodyConstraints.FreezeRotationZ;
    }

    public void LoseControl()
    {
        _controlsSelf = false;
        _canJump = false;
        _canRotate = false;
        if (_jumpCoroutine != null)
        {
            StopCoroutine(_jumpCoroutine);
            _jumpCoroutine = null;
        }
        if (_jumpResetCoroutine != null)
        {
            StopCoroutine(_jumpResetCoroutine);
            _jumpResetCoroutine = null;
        }
        _rb.constraints = RigidbodyConstraints.None;
    }

    private IEnumerator JumpAfterControl()
    {
        yield return new WaitForSeconds(_jumpCooldownAfterControlGain);
        _canJump = true;
        _jumpResetCoroutine = null;
    }
}
