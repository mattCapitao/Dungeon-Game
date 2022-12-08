using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Npc : ITarget // INHERITANCE
{
    private StateMachine _stateMachine;

    public Pillar TargetPillar { get; set; } // ENCAPSULATION
    public Npc TargetNpc { get; set; }


    public ITarget Target { get; set; }


    public int _attackDamage = 1;

    public Vector3 deathLaunchVelocity;

    private void Awake()
    {
        _maxHealth = 3;
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponentInChildren<Animator>();

        _stateMachine = new StateMachine();

        var search = new SearchForTarget(this);
        var moveToTarget = new MoveToCurrentTarget(this, navMeshAgent, animator);
        var attack = new AttackCurrentTarget(this, animator);
        var die = new DestroyNpc(this, navMeshAgent, animator);

        At(search, moveToTarget, HasTarget());
        At(moveToTarget, attack, ReachedTarget());
        At(attack, search, TargetDestroyed());
        At(moveToTarget, search, StuckTimeOut());

        _stateMachine.AddAnyTransition(die, () => isDestroyed);

        _stateMachine.SetState(search);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);


        Func<bool> HasTarget() => () => Target != null && !Target.isDestroyed;

        Func<bool> ReachedTarget() => () => Target != null && !Target.isDestroyed &&
            Vector3.Distance(transform.position, Target.transform.position) < 1.75f;
        
        Func<bool> TargetDestroyed() => () => Target == null || Target.isDestroyed ;

        Func<bool> StuckTimeOut() => () => moveToTarget.TimeStuck > 1f;

    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public override void TakeDamage(int dmg, Vector3 launchVelosity) // POLYMORPHISM
    {
        deathLaunchVelocity = launchVelosity;
        _health -= dmg;
        if (_health <= 0)
            Die(); // ABSTRACTION 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.IsChildOf(transform) || !other.CompareTag("Npc")) return;

        var npcTarget = other.gameObject.GetComponent<Npc>();

        if (ownedByPlayer != npcTarget.ownedByPlayer)
        {
            if (npcTarget != null && !npcTarget.isDestroyed)
            {
                Target = npcTarget;
                return;
            }
        }
    }
}
