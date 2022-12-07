using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Npc : ITarget
{
   // public new int _maxHealth = 3;

    private StateMachine _stateMachine;

    public Pillar TargetPillar { get; set; }
    public Npc TargetNpc { get; set; }


    public int _attackDamage = 1;

    public Vector3 deathLaunchVelocity;

    private void Awake()
    {
        _maxHealth = 3;
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
       // At(moveToTargetNpc, search, StuckTimeOutNpc());
       // At(moveToTargetPillar, search, StuckTimeOutPillar());
        At(attackNpc, search, TargetNpcDestroyed());
        At(attackPillar, search, TargetPillarDestroyed());

        _stateMachine.AddAnyTransition(die, () => isDestroyed);

        _stateMachine.SetState(search);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        Func<bool> HasTargetNpc() => () => TargetNpc != null && !TargetNpc.isDestroyed;
        Func<bool> HasTargetPillar() => () => TargetPillar != null && !TargetPillar.isDestroyed;

       // Func<bool> StuckTimeOutNpc() => () => moveToTargetNpc.TimeStuck > 1f;
       // Func<bool> StuckTimeOutPillar() => () => moveToTargetNpc.TimeStuck > 1f;

        Func<bool> ReachedTargetNpc() => () => TargetNpc != null && !TargetNpc.isDestroyed &&
                                              Vector3.Distance(transform.position, TargetNpc.transform.position) < 1.75f;

        Func<bool> ReachedTargetPillar() => () => TargetPillar != null && !TargetPillar.isDestroyed &&
                                               Vector3.Distance(transform.position, TargetPillar.transform.position) < 1.75f;

        Func<bool> TargetNpcDestroyed() => () => (TargetNpc == null || TargetNpc.isDestroyed) && (TargetPillar == null || TargetPillar.isDestroyed) ;
        Func<bool> TargetPillarDestroyed() => () => (TargetPillar == null || TargetPillar.isDestroyed) && (TargetNpc == null || TargetNpc.isDestroyed);

    }

    private void Update()
    {
        _stateMachine.Tick();

    }
    /*
    private void OnEnable()
    {
        _health = _maxHealth;
    }
    */
    public new void TakeDamage(int dmg, Vector3 launchVelosity)
    {
        // GetComponentInChildren<Animator>().SetTrigger("Impact");
        _health -= dmg;
        if (_health <= 0)
            Die(launchVelosity);
    }

    public new void Die(Vector3 launchVelosity)
    {
        if (launchVelosity == null) launchVelosity = Vector3.zero;
        if (isDestroyed) return;
        deathLaunchVelocity = launchVelosity;
        isDestroyed = true;
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.IsChildOf(transform) || !other.CompareTag("Npc")) return;

        var npcTarget = other.gameObject.GetComponent<Npc>();

        if (ownedByPlayer != npcTarget.ownedByPlayer)
        {
            
            if (npcTarget != null && !npcTarget.isDestroyed)
            {
                TargetPillar = null;
                TargetNpc = npcTarget;
                return;
            }

            TargetNpc = null;
        }
    }
}
