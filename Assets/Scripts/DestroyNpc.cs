using UnityEngine;
using UnityEngine.AI;

internal class DestroyNpc : IState
{
    private Npc _npc;

    public DestroyNpc(Npc npc)
    {
        _npc = npc;

    }

    public void Tick() { }

    public void OnEnter() {

        _npc.currentState = "destroy";
        _npc.dieclip.Play();
        _npc.isDestroyed = true;
        _npc.GetComponent<NavMeshAgent>().enabled = false;
        _npc.GetComponent <Animator>().enabled = false;
        _npc.GetComponent<CapsuleCollider>().enabled = false;
        _npc.GetComponent<SphereCollider>().enabled = false;

        var rigidbodies = _npc.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            rb.velocity = _npc.deathLaunchVelocity;
        }

        if (_npc.ownedByPlayer)
        {
            SpawnManager.Instance.blueSpawnCount--;
        }
        else
        {
            SpawnManager.Instance.redSpawnCount--;
        }

        GameObject.Destroy(_npc.gameObject, 2f);

    }

    public void OnExit() { }
}