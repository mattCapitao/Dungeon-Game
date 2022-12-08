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
    ITarget _target;
    float _nextAttackTime;
    int _attackDamage = 3;

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
        _animator.SetBool("Walk", _navMeshAgent.velocity.magnitude > 0.001f);
        

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
        if (_target.isDestroyed) { _target = null; return false; }

        float distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

        if (distanceToTarget > _attackDistance)
            return false;

        if (Time.time < _nextAttackTime)
            return false;

        return true;
    }

    private void AttackTarget()
    {
        _navMeshAgent.enabled = false;
        _nextAttackTime = Time.time + _attackDelay;
        _animator.SetTrigger("Attack");
        Vector3 launchVelosity = transform.forward + transform.up;
        launchVelosity *= _launchPower;

        StartCoroutine(DamageEnemy(launchVelosity)); 
    }

    IEnumerator DamageEnemy(Vector3 launchVelosity) // delay to time damage with attack animation
    {
        yield return new WaitForSeconds(.5f);

        if(_target != null)
        {
            if (_target.GetComponent<Npc>())
                _target = _target.GetComponent<Npc>();

            _target.TakeDamage(_attackDamage, launchVelosity);
        }
        
    }
    

    private void MoveToTarget()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_target.transform.position);
        if(_target!=null)
            FaceDestination(_target.transform.position);
    }

    private void FaceDestination(Vector3 location)
    {
        if (location == null) return;
        var targetRotation = Quaternion.LookRotation(location - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4 * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, targetRotation.eulerAngles.y, 0f)), .001f * Time.deltaTime);
    }


    void HandleClick()
    {
        _navMeshAgent.enabled = true;

        Ray ray = _camMain.ScreenPointToRay(Input.mousePosition);

        int hits = Physics.RaycastNonAlloc(ray, _results);

        if (TrySetAttackTarget(hits))
            return;

        TrySetGroundTarget(hits);
    }

    private bool ValidAttackTarget(ITarget enemy)
    {
        if (enemy != null && !enemy.ownedByPlayer && !enemy.isDestroyed)
        {
            _target = enemy;
            return true;
        }
        return false;
    }

    private bool TrySetAttackTarget(int hits)
    {
        ITarget enemy;
        for (int i = 0; i < hits; i++)
        {
             enemy = _results[i].collider.GetComponentInParent<Npc>();
            if (ValidAttackTarget(enemy))
                return true;

             enemy = _results[i].collider.GetComponentInChildren<Pillar>();
            if (ValidAttackTarget(enemy))
                return true;

            enemy = _results[i].collider.GetComponent<Tower>();
            if (ValidAttackTarget(enemy))
                return true;

            enemy = _results[i].collider.GetComponent<Castle>();
            if (ValidAttackTarget(enemy))
                return true;

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
