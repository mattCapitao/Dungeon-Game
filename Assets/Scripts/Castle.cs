using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : ITarget
{
    [SerializeField] Tower _tower;

    public override void TakeDamage(int dmg, Vector3 launchVelosity)
    {
        if (!_tower.isDestroyed)
        {
            Debug.Log("This castle cannot be damaged while its tower is intact");
            return;
        }

        _health -= dmg;
        if (_health <= 0)
            Die();
    }

    public override void Die()
    {
        isDestroyed = true;
       

        if (!ownedByPlayer)
        {
            Debug.Log("Player Victory");
        }
        else
        {
            Debug.Log("Player Defeat");
        }

        GetComponentInChildren<MeshRenderer>().enabled = false;
    }
}
