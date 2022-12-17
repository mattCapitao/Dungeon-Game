using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Npc : ITarget // INHERITANCE
{
    private StateMachine _stateMachine;

    

    /*
    public Pillar TargetPillar { get; set; } // ENCAPSULATION
    public Npc TargetNpc { get; set; }
    */

    public ITarget Target { get; set; }

    public ITarget currentTarget;
    public ITarget lastTarget;
    public string currentState;

    public float meleAttackRange = 2.5f;


    public int _attackDamage = 1;

    public Vector3 deathLaunchVelocity;

    private void Awake()
    {
        SetMaxHealth();
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponentInChildren<Animator>();

        _stateMachine = new StateMachine();

        var search = new SearchForTarget(this);
        var moveToTarget = new MoveToCurrentTarget(this, navMeshAgent, animator);
        var attack = new AttackCurrentTarget(this, animator);

        var clearTargets = new ClearTargets(this);

        At(search, moveToTarget, TargetOutOfMeleRange());
        At(search, attack, TargetInMeleRange());

        At(attack, search, TargetDestroyed());
        At(attack, moveToTarget, TargetOutOfMeleRange());
       
        At(moveToTarget, attack, TargetInMeleRange());
        At(moveToTarget, search, TargetDestroyed());
        At(moveToTarget, clearTargets, StuckTimeOut());

        At(clearTargets, search, TargetsClear());

        _stateMachine.SetState(search);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);


        Func<bool> TargetInMeleRange() => () => !isDestroyed && Target != null && !Target.isDestroyed &&
            Vector3.Distance(transform.position, Target.transform.position) <= meleAttackRange;

        Func<bool> TargetOutOfMeleRange() => () => !isDestroyed && Target != null && !Target.isDestroyed &&
            Vector3.Distance(transform.position, Target.transform.position) > meleAttackRange;

        Func<bool> TargetDestroyed() => () => !isDestroyed && Target.isDestroyed ;

        Func<bool> StuckTimeOut() => () => !isDestroyed && moveToTarget.TimeStuck > 3f;

        Func<bool> TargetsClear() => () => !isDestroyed && Target == null && currentTarget == null && lastTarget == null;

    }

    protected virtual void SetMaxHealth()
    {
        _maxHealth = 3;
    }

    private void Update()
    {
        _stateMachine.Tick();
        currentTarget = Target;
    }

    public override void TakeDamage(int dmg, Vector3 launchVelosity) // POLYMORPHISM
    {
        hitclip.Play();
        deathLaunchVelocity = launchVelosity;
        _health -= dmg;
        if (_health <= 0)
            Die();
    }

    public override void Die()
    {
        isDestroyed = true;
        currentState = "destroy";
        dieclip.Play();
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponentInChildren<Animator>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;

        var rigidbodies = this.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            rb.velocity = this.deathLaunchVelocity;
        }

        if (this.ownedByPlayer)
        {
            SpawnManager.Instance.blueSpawnCount--;
        }
        else
        {
            SpawnManager.Instance.redSpawnCount--;
        }

        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null || Target == null) return;

        if (other.transform.IsChildOf(transform) || !other.CompareTag("Npc") || Target.CompareTag("Npc")) return;

        var npcTarget = other.gameObject.GetComponent<Npc>();

        if (ownedByPlayer != npcTarget.ownedByPlayer)
        {
            if (npcTarget != null && !npcTarget.isDestroyed)
            {
                if (Target != null)
                    lastTarget = Target;
                Target = npcTarget;
                return;
            }
        }
    }
}
