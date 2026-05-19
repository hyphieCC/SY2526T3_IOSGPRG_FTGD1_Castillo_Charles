using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _spawnLocation;

    private List<GameObject> _enemies = new List<GameObject>();

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(_enemyPrefab, _spawnLocation.transform.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        //enemyScript.Initialize();
        _enemies.Add(enemy);
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        _enemies.Remove(enemy.gameObject);
    }
}
