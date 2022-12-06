using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float _nextSpawnTime;
    private GameObject _prefab;


    [SerializeField] float spawnDelay = 3f;
   
    [SerializeField] GameObject[] _spawnPoints;

    void Update()
    {
        if (ReadyToSpawn())
            Spawn();  
    }

    private bool ReadyToSpawn() => Time.time >= _nextSpawnTime;

    private void Spawn()
    {
        
        _nextSpawnTime = Time.time + spawnDelay;

        GameObject _spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        SpawnPoint spawnPointController = _spawnPoint.GetComponent<SpawnPoint>();

        _prefab = spawnPointController.prefabs[0];

        if (spawnPointController.ownedByPlayer)
        {
            _prefab = spawnPointController.prefabs[1];
            
        }
        var _npc = Instantiate(_prefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation);

        _prefab.GetComponent<Npc>().ownedByPlayer = spawnPointController.ownedByPlayer;
    }
}
