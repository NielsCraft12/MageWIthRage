using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : Enemy
{
    [Header("Dependencies")]
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _cosmeticGhostHead;
    [SerializeField] private GhostHead _ghostHead;
    [SerializeField] private Transform _ghostHand;
    [SerializeField] private Transform _player;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _bodyAnimator;
    [SerializeField] private Animator _headAnimator;
    [SerializeField] private Transform[] _patrolPoints;

    [Header("Settings")]
    [SerializeField] private float _throwVelocity = 5f;
    [SerializeField] private float _throwCooldown = 2f;
    [SerializeField] private float _throwMercyTime = 0.5f;
    [SerializeField] private float _maxDistanceMult = 2f;
    [SerializeField] private float _minDistanceMult = 1f;
    [Range(0, 5)][SerializeField] private float _rotateSpeed = 1f;

    [SerializeField][ReadOnly] private int _currentPatrolPoint = 0;

    [SerializeField] private float _headPickupTime = 0.5f;
    [SerializeField] private float _headThrowTime = 0.5f;
    private Vector3 lookDir;
    private List<Vector3> _patrolVectors;
    private bool _canThrow = true;
    private bool _canRetrieve = false;
    private bool _navMeshEnabled = true;
    private Coroutine _IncreasePatrolPointCoroutine;

    private void Awake()
    {
        _patrolVectors = new List<Vector3>();
        for (int i = 0; i < _patrolPoints.Length; i++)
        {
            _patrolVectors.Add(_patrolPoints[i].position);
            Debug.Log("Patrol Vector " + i + ": " + _patrolVectors[i]);
        }
        _navMeshAgent.SetDestination(_patrolVectors[_currentPatrolPoint]);
        _headAnimator.SetTrigger("Walk");
    }

    void Update()
    {
        if (!_navMeshEnabled && lookDir != transform.eulerAngles)
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
            case AlertState.ToOrigin:
                Idle();
                break;
            case AlertState.ToPlayer:
                ToPlayer();
                return;
        }
    }

    void Idle()
    {
        if (_IncreasePatrolPointCoroutine == null && Vector3.Distance(transform.position, _patrolVectors[_currentPatrolPoint]) < 1f)
        {
            _IncreasePatrolPointCoroutine = StartCoroutine(IncreasePatrolPoint());
        }
        if (!_navMeshEnabled)
        {
            TogglePhysics();
        }
    }

    void ToPlayer()
    {
        DirectionToTarget(_player.position);
        if (_canThrow)
            StartCoroutine(HeadPickupToThrow());
        if (_navMeshEnabled)
        {
            TogglePhysics();
        }
    }

    IEnumerator IncreasePatrolPoint()
    {
        _currentPatrolPoint++;
        if (_currentPatrolPoint >= _patrolVectors.Count)
            _currentPatrolPoint = 0;

        yield return new WaitForSeconds(1f);
        if (_navMeshEnabled)
            _navMeshAgent.SetDestination(_patrolVectors[_currentPatrolPoint]);
        _IncreasePatrolPointCoroutine = null;
    }

    void DirectionToTarget(Vector3 target)
    {
        lookDir = target - transform.position;
        lookDir.y = 0;
    }

    private void TogglePhysics()
    {
        Vector3 velocity;
        if (_navMeshAgent.enabled)
            velocity = _navMeshAgent.velocity;
        else
            velocity = _rigidbody.linearVelocity;

        _navMeshAgent.enabled = !_navMeshAgent.enabled;
        _navMeshEnabled = !_navMeshEnabled;
        if (_navMeshAgent.enabled) _navMeshAgent.SetDestination(_patrolVectors[_currentPatrolPoint]);
        _rigidbody.isKinematic = !_rigidbody.isKinematic;

        if (_rigidbody.isKinematic)
            _navMeshAgent.velocity = velocity;
        else
            _rigidbody.linearVelocity = velocity;
    }

    IEnumerator HeadPickupToThrow()
    {
        _ghostHead.rb.isKinematic = true;
        _bodyAnimator.SetTrigger("Attack");
        _canThrow = false;
        _canRetrieve = false;
        yield return new WaitForSeconds(_headPickupTime);
        _cosmeticGhostHead.SetActive(false);
        _head.transform.parent = _ghostHand;
        _head.transform.position = _ghostHand.position;
        _head.SetActive(true);
        _headAnimator.SetTrigger("Throw");
        yield return new WaitForSeconds(_headThrowTime);
        _head.transform.parent = null;
        _ghostHead.rb.isKinematic = false;
        ThrowGhostHead();
    }

    void ThrowGhostHead()
    {
        float distanceToPlayer = Vector3.Distance(_player.position, transform.position) / 7f;
        distanceToPlayer = Mathf.Clamp(distanceToPlayer, _minDistanceMult, _maxDistanceMult);
        _ghostHead.rb.linearVelocity = (_player.position - transform.position).normalized * distanceToPlayer * _throwVelocity;
        StartCoroutine(MercyTime());
    }

    IEnumerator CollectGhostHead()
    {
        Debug.Log("Ghost head collected!");
        _ghostHead.rb.isKinematic = true;
        _head.transform.parent = _ghostHand;
        _head.transform.localPosition = Vector3.zero;
        _bodyAnimator.SetTrigger("Catch");
        yield return new WaitForSeconds(_throwCooldown);
        _head.SetActive(false);
        _cosmeticGhostHead.SetActive(true);
        _headAnimator.SetTrigger("Walk");
        StartCoroutine(ThrowCooldown());
    }

    IEnumerator ThrowCooldown()
    {
        yield return new WaitForSeconds(_throwCooldown);
        _canThrow = true;
    }

    IEnumerator MercyTime()
    {
        yield return new WaitForSeconds(_throwMercyTime);
        _canRetrieve = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (_canRetrieve && other.gameObject == _head)
        {
            StartCoroutine(CollectGhostHead());
        }
    }
}
