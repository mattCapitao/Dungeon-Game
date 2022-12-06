using System;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private float _nextToggleTime;
    bool _trapActive;

    [SerializeField] List<Npc> _enemies;
    [SerializeField] float _toggleRate = 2f;

    public bool ownedByPlayer;
    

    private void Update()
    {
        if (ShoudToggleTrap())
            ToggleTrap();
    }

    private bool ShoudToggleTrap() => Time.time >= _nextToggleTime;
    
    void ToggleTrap()
    {
        _nextToggleTime = Time.time + _toggleRate;
        _trapActive = !_trapActive;
        GetComponent<Animator>().SetBool("Active", _trapActive);

        if (_trapActive)
            TriggerTrap();
    }

    public void TriggerTrap()
    {
        foreach (var enemy in _enemies)
            enemy.Die(Vector3.up * 5);
    }
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Npc>();
        if (enemy != null)
            _enemies.Add(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponent<Npc>();
        if (enemy != null)
            _enemies.Remove(enemy);
    }
}
