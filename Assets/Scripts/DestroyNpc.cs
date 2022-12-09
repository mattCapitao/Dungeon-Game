using UnityEngine;
using UnityEngine.AI;

internal class DestroyNpc : IState
{
    private Npc _npc;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    public DestroyNpc(Npc npc, NavMeshAgent navMeshAgent, Animator animator)
    {
        _npc = npc;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
    }


    public void Tick() { }

    public void OnEnter() {
        GameObject.Destroy(_npc.gameObject, 2f);
        _navMeshAgent.enabled = false;
        _animator.enabled = false;
        _npc.GetComponent<CapsuleCollider>().enabled = false;
        _npc.GetComponent<SphereCollider>().enabled = false;

        var rigidbodies = _npc.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            rb.velocity = _npc.deathLaunchVelocity;
        }
        
    }

    public void OnExit() { }
}