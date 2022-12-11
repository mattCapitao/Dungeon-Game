using UnityEngine;
using UnityEngine.AI;

internal class MoveToCurrentTarget : IState
{
    private Npc _npc;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private Vector3 _lastPosition = Vector3.zero;

    public float TimeStuck;

    public MoveToCurrentTarget(Npc npc, NavMeshAgent navMeshAgent, Animator animator)
    {
       _npc = npc;
       _navMeshAgent = navMeshAgent;
       _animator = animator;

    }

    public void Tick()
    {
        if (Vector3.Distance(_npc.transform.position, _lastPosition) <= .001f)
            TimeStuck += Time.deltaTime;
        
        _lastPosition = _npc.transform.position;
    }

    public void OnEnter()
    {
        _npc.currentState = "moveToTarget";
        TimeStuck = 0f;
        _navMeshAgent.enabled = true;
        _animator.SetBool("Walk", true);

        if (_npc.Target != null && !_npc.Target.isDestroyed)
        {
            _navMeshAgent.SetDestination(_npc.Target.transform.position);
            return;
        }
    }

    public void OnExit()
    {
        _npc.currentState = "Exited moveToTarget";
        _navMeshAgent.enabled = false;
        _animator.SetBool("Walk", false);
    }
}