using UnityEngine;
using UnityEngine.AI;

internal class DestroyNpc : IState
{
    private Npc _npcController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    public DestroyNpc(Npc npcController, NavMeshAgent navMeshAgent, Animator animator)
    {
        _npcController = npcController;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
    }


    public void Tick() { }

    public void OnEnter() {
        _navMeshAgent.enabled = false;
        _animator.enabled = false;
        _npcController.GetComponent<Collider>().enabled = false;

        var rigidbodies = _npcController.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            rb.velocity = _npcController.deathLaunchVelocity;
        }
        GameObject.Destroy(_npcController.gameObject, 5f);
    }

    public void OnExit() { }
}