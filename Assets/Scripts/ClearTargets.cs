using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal  class ClearTargets :  IState
{
    private Npc _npc;

    public ClearTargets(Npc npc)
    {
        _npc = npc;
    }

    public void Tick(){ }

    public void OnEnter()
    {
        _npc.currentState = "clear targets";
        _npc.Target = null;
        _npc.currentTarget = null;
        _npc.lastTarget = null;
    }

    public void OnExit()
    {
        _npc.currentState = "Exited clear targets";
    }

}
