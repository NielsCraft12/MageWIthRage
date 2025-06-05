using System.Collections;
using UnityEngine;

public class Ghost : Enemy
{
    [Header("Dependencies")]
    [SerializeField] private GameObject _head;
    [SerializeField] private GhostHead _ghostHead;
    [SerializeField] private Transform _player;

    [Header("Settings")]
    [SerializeField] private float _throwVelocity = 5f;
    [SerializeField] private float _throwCooldown = 2f;
    [SerializeField] private float _throwMercyTime = 0.5f;
    [SerializeField] private float _maxDistanceMult = 2f;
    [SerializeField] private float _minDistanceMult = 1f;

    private bool _canThrow = true;
    private bool _canRetrieve = false;

    void FixedUpdate()
    {
        switch (alertState)
        {
            case AlertState.Idle:
                return;
            case AlertState.ToPlayer:
                if (_canThrow)
                    ThrowGhostHead();
                return;
            case AlertState.ToOrigin:
                break;
        }
    }

    void ThrowGhostHead()
    {
        float distanceToPlayer = Vector3.Distance(_player.position, transform.position) / 7f;
        distanceToPlayer = Mathf.Clamp(distanceToPlayer, _minDistanceMult, _maxDistanceMult);
        Debug.Log(distanceToPlayer);
        _canThrow = false;
        _canRetrieve = false;
        _head.SetActive(true);
        _head.transform.position = transform.position;
        _ghostHead.rb.linearVelocity = (_player.position - transform.position).normalized * distanceToPlayer * _throwVelocity;
        StartCoroutine(MercyTime());
    }

    void CollectGhostHead()
    {
        _head.transform.position = transform.position;
        _head.SetActive(false);
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

    void OnTriggerEnter(Collider other)
    {
        if (_canRetrieve && other.gameObject == _head)
        {
            CollectGhostHead();
        }
    }
}
