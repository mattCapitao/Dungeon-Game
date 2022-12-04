using System;
using System.Collections;
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

    [SerializeField] float _attackDistance = .5f;
    [SerializeField] float _attackDelay = 2f;
    [SerializeField] float _launchPower = 3.5f;

    void Awake()
    {
        _camMain = Camera.main;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        _animator.SetBool("Walk", _navMeshAgent.velocity.magnitude > 0);
        

        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0) && _target == null)
            {
                FaceDestination(_navMeshAgent.destination);
            }

            HandleClick();
        }
        else if (_target != null)
        {
            MoveToTarget();
        }

        if (ReadyToAttack())
            AttackTarget();

    }

    private bool ReadyToAttack()
    {
        if (_target == null) return false;

        float distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

        if (distanceToTarget > _attackDistance)
            return false;

        if (Time.time < _nextAttackTime)
            return false;

        return true;
    }

    private void AttackTarget()
    {
        
        _nextAttackTime = Time.time + _attackDelay;
        _animator.SetTrigger("Attack");
        var launchVelosity = transform.forward + transform.up;
        launchVelosity *= _launchPower;

        StartCoroutine(KillEnemy(_target, launchVelosity));
        _target = null;
    }

    IEnumerator KillEnemy(Enemy target, Vector3 launchVelosity)
    {
        yield return new WaitForSeconds(.5f);
        target.Die(launchVelosity);
    }

    private void MoveToTarget()
    {
        _navMeshAgent.SetDestination(_target.transform.position);

        FaceDestination(_target.transform.position);
    }

    private void FaceDestination(Vector3 loc)
    {
        var targetRotation = Quaternion.LookRotation(loc - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4 * Time.deltaTime);
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
