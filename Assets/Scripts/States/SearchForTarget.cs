

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

    private void SelectBestTarget() 
    {

        if (TryGetHeroTarget()) return;
        if (TryGetCastleTarget()) return;
        if (TryGetTowerTarget()) return;
        if (TryGetPillarTarget()) return;

    }

    private bool TryGetHeroTarget()
    {
        Npc npcTarget = Object.FindObjectsOfType<NpcHero>()
                .OrderBy(npc => Vector3.Distance(_npc.gameObject.transform.position, npc.transform.position))
                 .Where(npc => (npc.ownedByPlayer != _npc.ownedByPlayer) && (npc.isDestroyed == false) &&
                 Vector3.Distance(_npc.gameObject.transform.position, npc.transform.position) <= 10)
                 .Take(1)
                 .OrderBy(npc => Random.Range(0, int.MaxValue))
                 .FirstOrDefault();
        if (npcTarget != null)
        {
            _npc.Target = npcTarget;
            return true;
        }
        return false;
    }

    private bool TryGetCastleTarget()
    {
        Castle castleTarget = Object.FindObjectsOfType<Castle>()
                    .OrderBy(castle => Vector3.Distance(_npc.gameObject.transform.position, castle.transform.position))
                    .Where(castle => (castle.ownedByPlayer != _npc.ownedByPlayer) && (castle.isDestroyed == false) && (castle.tower.isDestroyed) )
                    .OrderBy(castle => Random.Range(0, int.MaxValue))
                    .Take(1)
                    .FirstOrDefault();
        if (castleTarget != null)
        {

            _npc.Target = castleTarget;
            return true;
        }
        return false;
    }

    private bool TryGetTowerTarget()
    {
        Tower towerTarget = Object.FindObjectsOfType<Tower>()
                    .OrderBy(tower => Vector3.Distance(_npc.gameObject.transform.position, tower.transform.position))
                    .Where(tower => (tower.ownedByPlayer != _npc.ownedByPlayer) && tower.isDestroyed == false && tower.pillar.isDestroyed)
                    .OrderBy(tower => Random.Range(0, int.MaxValue))
                    .Take(1)
                    .FirstOrDefault();
        if (towerTarget != null)
        {

            _npc.Target = towerTarget;
            return true;
        }
        return false;
    }

    private bool TryGetPillarTarget()
    {
        Pillar pillarTarget = Object.FindObjectsOfType<Pillar>()
                    .OrderBy(pillar => Vector3.Distance(_npc.gameObject.transform.position, pillar.transform.position))
                    .Where(pillar => (pillar.ownedByPlayer != _npc.ownedByPlayer) && (pillar.isDestroyed == false))
                    .OrderBy(pillar => Random.Range(0, int.MaxValue))
                    .Take(1)
                    .FirstOrDefault();
        if (pillarTarget != null)
        {

            _npc.Target = pillarTarget;
            return true;
        }
        return false;
    }

    public void OnEnter() {

    }
    public void OnExit() { }
}
