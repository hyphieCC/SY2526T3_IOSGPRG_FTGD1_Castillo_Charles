using Castillo.Combat;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Castillo.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy")]
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private int _enemyCount = 20;

        [Header("World Bounds")]
        [SerializeField] private Vector2 _minimumWorldPosition = new Vector2(-95f, -45f);
        [SerializeField] private Vector2 _maximumWorldPosition = new Vector2(95f, 45f);

        [Header("Spawn Validation")]
        [SerializeField] private float _spawnCheckRadius = 0.75f;
        [SerializeField] private float _minimumDistanceBetweenEnemies = 2f;
        [SerializeField] private int _maximumSpawnAttempts = 30;
        [SerializeField] private LayerMask _blockedLayers;

        private readonly List<Vector2> _spawnedPositions = new List<Vector2>();

        public int AliveEnemyCount { get; private set; }

        public event Action AllEnemiesDefeated;
        public event Action<int> AliveEnemyCountChanged;

        private void Start()
        {
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < _enemyCount; i++)
            {
                TrySpawnEnemy();
            }
        }

        private void TrySpawnEnemy()
        {
            for (int i = 0; i < _maximumSpawnAttempts; i++)
            {
                Vector2 spawnPosition = GetRandomSpawnPosition();

                if (!IsSpawnPositionValid(spawnPosition))
                {
                    continue;
                }

                GameObject enemyObject = Instantiate(
                    _enemyPrefab,
                    spawnPosition,
                    Quaternion.identity,
                    transform
                );

                Health enemyHealth = enemyObject.GetComponent<Health>();

                if (enemyHealth != null)
                {
                    enemyHealth.Died += OnEnemyDied;
                }

                AliveEnemyCount++;
                AliveEnemyCountChanged?.Invoke(AliveEnemyCount);
                _spawnedPositions.Add(spawnPosition);
                return;
            }

            Debug.LogWarning($"{nameof(EnemySpawner)} could not find " + "a valid enemy spawn position.");
        }

        private Vector2 GetRandomSpawnPosition()
        {
            float randomX = UnityEngine.Random.Range(
                _minimumWorldPosition.x,
                _maximumWorldPosition.x
            );

            float randomY = UnityEngine.Random.Range(
                _minimumWorldPosition.y,
                _maximumWorldPosition.y
            );

            return new Vector2(randomX, randomY);
        }

        private bool IsSpawnPositionValid(Vector2 position)
        {
            Collider2D blockedCollider = Physics2D.OverlapCircle(
                position,
                _spawnCheckRadius,
                _blockedLayers
            );

            if (blockedCollider != null)
            {
                return false;
            }

            foreach (Vector2 spawnedPosition in _spawnedPositions)
            {
                float distanceSquared = (spawnedPosition - position).sqrMagnitude;

                float minimumDistanceSquared =
                    _minimumDistanceBetweenEnemies *
                    _minimumDistanceBetweenEnemies;

                if (distanceSquared < minimumDistanceSquared)
                {
                    return false;
                }
            }

            return true;
        }

        private void OnEnemyDied()
        {
            AliveEnemyCount--;
            AliveEnemyCountChanged?.Invoke(AliveEnemyCount);

            if (AliveEnemyCount > 0)
            {
                return;
            }

            AllEnemiesDefeated?.Invoke();
        }
    }
}