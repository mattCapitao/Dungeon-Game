using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    [SerializeField] int _maxHealth = 100;
    int _health;

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
        SceneManager.LoadScene(0);
    }
}
