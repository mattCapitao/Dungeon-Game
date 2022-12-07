using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITarget : MonoBehaviour
{
    public int _maxHealth = 0;
    public int _health;

    public bool isDestroyed;
    public bool ownedByPlayer;


    private void OnEnable()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int dmg, Vector3 launchVelosity)
    {
        if (launchVelosity == null) launchVelosity = Vector3.zero;
        _health -= dmg;
        if (_health <= 0)
            Die(launchVelosity);
    }

    public void Die(Vector3 launchVelosity)
    {
        isDestroyed = true;
        if(GetComponent<Pillar>())
            GetComponent<MeshRenderer>().enabled = false;
        //SceneManager.LoadScene(0);
    }
}
