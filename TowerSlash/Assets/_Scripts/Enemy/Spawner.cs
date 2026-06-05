using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform[] _spawnLocation;

    [Header("Spawn Time")]
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;

    private List<GameObject> _enemies = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(CO_SpawnEnemy());
    }

    public void SpawnEnemy()
    {
        int randomSpawn = Random.Range(0, _spawnLocation.Length);
        Transform selectedSpawnLocation = _spawnLocation[randomSpawn];

        GameObject enemy = Instantiate(_enemyPrefab, selectedSpawnLocation.position, Quaternion.identity);
        //Enemy enemyScript = enemy.GetComponent<Enemy>();
        //enemyScript.Initialize();
        _enemies.Add(enemy);
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        _enemies.Remove(enemy.gameObject);
    }

    private IEnumerator CO_SpawnEnemy()
    {
        while (true)
        {
            float randSpawnTime = Random.Range(_minSpawnTime, _maxSpawnTime);
            yield return new WaitForSeconds(randSpawnTime);
            SpawnEnemy();
        }
    }
}
