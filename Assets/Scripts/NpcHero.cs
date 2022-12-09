using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHero : Npc
{
    protected override void SetMaxHealth()
    {
        _maxHealth = 50;
    }
}
