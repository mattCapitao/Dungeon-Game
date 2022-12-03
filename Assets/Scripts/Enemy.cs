using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Flag _flag;
    private float _nextAttackTime;

    [SerializeField] float _attackDistance = 2;
    [SerializeField] float _attackDelay = 3;
    [SerializeField] int _attackDamage = 1;
    [SerializeField] float _launchPower = 3.5f;

    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

    }

    private void OnEnable()
    {
        _flag = FindObjectOfType<Flag>();
        _navMeshAgent.SetDestination(_flag.transform.position);
    }

    private void Update()
    {
        _animator.SetBool("Walk",_navMeshAgent.velocity.magnitude > 0);

        if (ReadyToAttack())
            Attack();
    }

    private bool ReadyToAttack()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _flag.transform.position);
       
        if (distanceToTarget > _attackDistance)
            return false;
        
        if (Time.time < _nextAttackTime)
            return false;

        return true;
    }

    private void Attack()
    {
        _nextAttackTime = Time.time + _attackDelay;
        _animator.SetTrigger("Attack");
        _flag.TakeDamage(_attackDamage);
    }

    [ContextMenu("Die")]

    public void Die()
    {
        _navMeshAgent.enabled = false;
        _animator.enabled = false;

        var launchVelosity = -transform.forward + transform.up;
        launchVelosity *= _launchPower;

        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rb in rigidbodies)
        {
            rb.velocity = launchVelosity;
        }
    }
}
