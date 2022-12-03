using System;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    RaycastHit[] _results = new RaycastHit[100];
    Camera _camMain;
    NavMeshAgent _navMeshAgent;
    Animator _animator;
    Enemy _target;
    float _nextAttackTime;

    [SerializeField] float _attackDistance = 1.5f;
    [SerializeField] float _attackDelay = 2f;

    void Awake()
    {
        _camMain = Camera.main;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        _animator.SetBool("Walk", _navMeshAgent.velocity.magnitude > 0);

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
        else if (_target != null)
        {
            MoveToTarget();
        }

        if (ReadyToAttack())
            Attack();
           
    }

    private bool ReadyToAttack()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

        if (distanceToTarget > _attackDistance)
            return false;

        if (Time.time < _nextAttackTime)
            return false;

        return true;
    }

    private void Attack()
    {
        _nextAttackTime = Time.time + _attackDelay;
        _target.Die();
    }

    private void MoveToTarget()
    {
        _navMeshAgent.SetDestination(_target.transform.position);
    }

    void HandleClick()
    {
        Ray ray = _camMain.ScreenPointToRay(Input.mousePosition);

        int hits = Physics.RaycastNonAlloc(ray, _results);

        if (TrySetEnemyTarget(hits))
            return;
        TrySetGroundTarget(hits);
    }

    private bool TrySetEnemyTarget(int hits)
    {
        for (int i = 0; i < hits; i++)
        {
            var enemy = _results[i].collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                _target = enemy;
                return true;
            }
        }
        return false;
    }

    private void TrySetGroundTarget(int hits)
    {
        for (int i = 0; i < hits; i++)
        {
            if (_navMeshAgent.SetDestination(_results[i].point))
            {
                _target = null;
                break;
            }
        }
    }
}
