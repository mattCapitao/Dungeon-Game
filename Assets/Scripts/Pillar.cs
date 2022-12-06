using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pillar : MonoBehaviour
{
    [SerializeField] int _maxHealth = 100;
    int _health;

    public bool isDestroyed;
    public bool ownedByPlayer;

    private void OnEnable()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        _health -= dmg;
        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        isDestroyed = true;
        GetComponent<MeshRenderer>().enabled = false; 
        //SceneManager.LoadScene(0);
    }
}
