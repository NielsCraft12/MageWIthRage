using System.Collections;
using UnityEngine;

public class Ghost : Enemy
{
    [Header("Dependencies")]
    [SerializeField] GameObject _head;
    [SerializeField] GhostHead _ghostHead;
    [SerializeField] private Transform _player;

    [Header("Settings")]
    [SerializeField] float _throwVelocity = 5f;
    [SerializeField] float _throwCooldown = 2f;
    [SerializeField] float _throwMercyTime = 0.5f;

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
        _canThrow = false;
        _canRetrieve = false;
        _head.SetActive(true);
        _head.transform.position = transform.position;
        _ghostHead.rb.linearVelocity = (_player.position - transform.position).normalized * _throwVelocity;
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
