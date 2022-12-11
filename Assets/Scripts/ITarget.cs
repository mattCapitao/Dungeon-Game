using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITarget : MonoBehaviour
{
    public AudioSource hitclip;
    public AudioSource dieclip;

    public int _maxHealth = 0;
    public int _health;

    public bool isDestroyed;
    public bool ownedByPlayer;

    protected void OnEnable()
    {
        _health = _maxHealth;
    }

    public virtual void TakeDamage(int dmg, Vector3 launchVelosity)
    {
        hitclip.Play();
        _health -= dmg;
        if (_health <= 0)
            Die();
    }

    public virtual void Die()
    {
        isDestroyed = true;
    }
}
