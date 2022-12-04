using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float _nextSpawnTime;

    [SerializeField] float spawnDelay = 3f;
    [SerializeField] GameObject _prefab;
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
        Instantiate(_prefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation);
    }
}
