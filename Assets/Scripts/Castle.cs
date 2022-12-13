using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Castle : ITarget
{
    public Tower[] towers;

    public override void TakeDamage(int dmg, Vector3 launchVelosity)
    {
        if (!AllTowersDestroyed())
        {
            Debug.Log("This castle cannot be damaged while any enemy tower is intact");
            return;
        }
        hitclip.Play();
        _health -= dmg;
        if (_health <= 0)
            Die();
    }

    public bool AllTowersDestroyed()
    {
       
       var standingTower = Object.FindObjectsOfType<Tower>()
             .Where(tower => (tower.ownedByPlayer == ownedByPlayer) && tower.isDestroyed == false)
             .FirstOrDefault();
        if (standingTower != null)
            return false;

        return true;
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

        
        
        gameObject.SetActive(false);

        /*
         *foreach (MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
         * 
        MeshRenderer[] meshRenderers = transform.parent.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
        */
    }
}
