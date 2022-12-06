using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Npc : MonoBehaviour
{
    [SerializeField] int _maxHealth = 3;

    private StateMachine _stateMachine;

    public Pillar TargetPillar {get;set;}
    public Npc TargetNpc { get; set; }


    public int _health;
    public int _attackDamage = 1;
    public bool isDestroyed;
    public bool ownedByPlayer;
    public Vector3 deathLaunchVelocity;

    private void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponentInChildren<Animator>();
       

        _stateMachine = new StateMachine();

        var search = new SearchForTarget(this);
        var moveToTargetNpc = new MoveToCurrentTarget(this, navMeshAgent, animator);
        var moveToTargetPillar = new MoveToCurrentTarget(this, navMeshAgent, animator);
        var attackNpc = new AttackCurrentTarget(this, animator);
        var attackPillar = new AttackCurrentTarget(this, animator);
        var die = new DestroyNpc(this, navMeshAgent, animator);

        At(search, moveToTargetNpc, HasTargetNpc());
        At(search, moveToTargetPillar, HasTargetPillar());
        At(moveToTargetNpc, attackNpc, ReachedTargetNpc());
        At(moveToTargetPillar, attackPillar, ReachedTargetPillar());
        At(attackPillar, search, TargetNpcDestroyed());
        At(attackPillar, search, TargetPillarDestroyed());

        _stateMachine.AddAnyTransition(die, () => isDestroyed);

        _stateMachine.SetState(search);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition( to, from, condition);

        Func<bool> HasTargetNpc() => () => TargetNpc != null;
        Func<bool> HasTargetPillar() => () => TargetPillar != null;
        Func<bool> ReachedTargetNpc() => () => TargetNpc != null &&
                                              Vector3.Distance(transform.position, TargetNpc.transform.position) < 2f;
        Func<bool> ReachedTargetPillar() => () => TargetPillar != null &&
                                               Vector3.Distance(transform.position, TargetPillar.transform.position) < 2f;

        Func<bool> TargetNpcDestroyed() => () => TargetNpc == null || 
                                                TargetNpc.isDestroyed;
        Func<bool> TargetPillarDestroyed() => () => TargetPillar == null ||
                                                TargetPillar.isDestroyed;

    }

    private void Update()
    {
        _stateMachine.Tick();

    }

    private void OnEnable()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        _health -= dmg;
        if (_health <= 0)
            Die(Vector3.forward * -3);
    }

    public void Die(Vector3 launchVelosity)
    {
        if (isDestroyed) return;
        deathLaunchVelocity = launchVelosity;
        isDestroyed = true;
    }



 

}
