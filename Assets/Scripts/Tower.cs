using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : ITarget
{
    public Pillar pillar;

    public override void TakeDamage(int dmg, Vector3 launchVelosity)
    {
        if (!pillar.isDestroyed)
        {
            Debug.Log("This tower cannot be damaged while its pillar is intact");
            return;
        }
        hitclip.Play();
        _health -= dmg;
        if (_health <= 0)
            Die();
    }

    public override void Die()
    {
        isDestroyed = true;
        gameObject.SetActive(false);
    }
}
