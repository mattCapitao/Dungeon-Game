using UnityEngine;

internal class AttackCurrentTarget : IState
{
    private Npc _npc;
    private Animator _animator;
    private float _hitsPerSecond = .33f;

    private float _nextAttackTime;
   

    public AttackCurrentTarget(Npc npc, Animator animator)
    {
        _npc = npc;
        _animator = animator;
    }

    public void Tick()
    {

        if (_npc.TargetNpc != null && !_npc.TargetNpc.isDestroyed  && _npc.ownedByPlayer != _npc.TargetNpc.ownedByPlayer)
        {
            if (_nextAttackTime <= Time.time)
            {
                _nextAttackTime = Time.time + (1f / _hitsPerSecond);
                _animator.SetTrigger("Attack");
                _npc.TargetNpc.TakeDamage(_npc._attackDamage, _npc.deathLaunchVelocity);
            }
        }

        if (_npc.TargetPillar != null && !_npc.TargetPillar.isDestroyed  && _npc.ownedByPlayer != _npc.TargetPillar.ownedByPlayer)
        {
            if (_nextAttackTime <= Time.time)
            {
                _nextAttackTime = Time.time + (1f / _hitsPerSecond);
                _animator.SetTrigger("Attack");
                _npc.TargetPillar.TakeDamage(_npc._attackDamage, Vector3.zero);
            }
        }
        
    }


    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }
}