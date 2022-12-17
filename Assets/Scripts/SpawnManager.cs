using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float _nextSpawnTime;
    private GameObject _prefab;

    private bool playerSpawn;
    private GameObject[] _spawnPoints;
    [SerializeField] float spawnDelay = 1f;
   
    [SerializeField] GameObject[] _playerSpawnPoints;
    [SerializeField] GameObject[] _enemySpawnPoints;

    public int redSpawnCount = 0;
    public int blueSpawnCount = 0;
    public int spawnCountMax = 50;

    public static SpawnManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (ReadyToSpawn())
            Spawn();  
    }

    private bool ReadyToSpawn() => Time.time >= _nextSpawnTime && spawnCountMax >= (redSpawnCount + blueSpawnCount);
    
    private void Spawn()
    {
        playerSpawn = !playerSpawn;

        _spawnPoints = _enemySpawnPoints;

        if (playerSpawn)
        {
           _spawnPoints = _playerSpawnPoints;
        }
        

        GameObject _spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        SpawnPoint spawnPointController = _spawnPoint.GetComponent<SpawnPoint>();

        Tower tower = spawnPointController.structure.GetComponent<Tower>();
        if(tower)
            if (spawnPointController.structure.GetComponent<Tower>().isDestroyed)return;

        
        if (spawnPointController.ownedByPlayer)
        {
            _prefab = spawnPointController.prefabs[1];
            blueSpawnCount++;
        }
        else
        {
            _prefab = spawnPointController.prefabs[0];
            redSpawnCount++;
        }

        _nextSpawnTime = Time.time + spawnDelay;

        var _npc = Instantiate(_prefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation);

        _prefab.GetComponent<Npc>().ownedByPlayer = spawnPointController.ownedByPlayer;
    }
}
