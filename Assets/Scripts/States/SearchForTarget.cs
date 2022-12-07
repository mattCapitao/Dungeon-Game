

using System.Linq;

using UnityEngine;

public class SearchForTarget :IState
{
    private readonly Npc _npc;

    public SearchForTarget(Npc npc)
    {
        _npc = npc;
    }

    public void Tick()
    {
        SelectBestTarget();
    }

    private void SelectBestTarget() // right now this is nearest enemy pillar only. it will be extended to include enemy NPCs Castles etc.
    {
       if (Random.Range(0,10) > 6) {
           Npc npcTarget = Object.FindObjectsOfType<Npc>()
               .OrderBy(npc => Vector3.Distance(_npc.gameObject.transform.position, npc.transform.position))
                .Where(npc => (npc.ownedByPlayer != _npc.ownedByPlayer) && (npc.isDestroyed == false) &&
                Vector3.Distance(_npc.gameObject.transform.position, npc.transform.position) <= 3)
                .Take(1)
                .OrderBy(npc => Random.Range(0, int.MaxValue))
                .FirstOrDefault();
            if (npcTarget != null)
            {
                _npc.TargetPillar = null;
                _npc.TargetNpc = npcTarget;
                return;
            }
        }


        Pillar pillarTarget =  Object.FindObjectsOfType<Pillar>()
            .OrderBy(pillar => Vector3.Distance(_npc.gameObject.transform.position, pillar.transform.position))
            .Where(pillar => (pillar.ownedByPlayer != _npc.ownedByPlayer)  && (pillar.isDestroyed == false))
            .OrderBy(pillar => Random.Range(0, int.MaxValue))
            .Take(1)
            .FirstOrDefault();
        if (pillarTarget != null)
        {
            _npc.TargetNpc = null;
            _npc.TargetPillar = pillarTarget;
            return;
        }


    }

    public void OnEnter() {

    }
    public void OnExit() { }
}
