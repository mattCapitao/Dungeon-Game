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
       Vector3 launchVelosity = Vector3.zero;

        if (_npc.Target != null && !_npc.Target.isDestroyed && _npc.ownedByPlayer != _npc.Target.ownedByPlayer && Vector3.Distance(_npc.transform.position, _npc.Target.transform.position) <= 1.75f)
        {
            if (_nextAttackTime <= Time.time)
            {
                _nextAttackTime = Time.time + (1f / _hitsPerSecond);
                _animator.SetTrigger("Attack");

                if (_npc.Target.GetComponent<Npc>())
                {
                    launchVelosity = _npc.deathLaunchVelocity;
                }

                _npc.Target.TakeDamage(_npc._attackDamage, _npc.deathLaunchVelocity);
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