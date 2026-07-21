using System.Collections.Generic;
using UnityEngine;

namespace Castillo.Loot
{
    public class LootSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private int _spawnCount = 30;
        [SerializeField] private float _weaponSpawnChance = 0.3f;
        [SerializeField] private float _minimumSpawnDistance = 2f;
        [SerializeField] private int _maximumSpawnAttempts = 20;

        [Header("World Bounds")]
        [SerializeField] private Vector2 _minimumWorldPosition = new Vector2(-95f, -45f);
        [SerializeField] private Vector2 _maximumWorldPosition = new Vector2(95f, 45f);

        [Header("Collision Check")]
        [SerializeField] private float _spawnCheckRadius = 0.5f;
        [SerializeField] private LayerMask _blockedLayers;

        [Header("Weapon Pickups")]
        [SerializeField] private List<GameObject> _weaponPickups;

        [Header("Ammo Pickups")]
        [SerializeField] private List<GameObject> _ammoPickups;

        private readonly List<Vector2> _spawnedPositions = new List<Vector2>();

        private void Start()
        {
            SpawnLoot();
        }

        private void SpawnLoot()
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                TrySpawnLoot();
            }
        }

        private void TrySpawnLoot()
        {
            for (int attempt = 0; attempt < _maximumSpawnAttempts; attempt++)
            {
                Vector2 spawnPosition = GetRandomPosition();

                if (!IsPositionValid(spawnPosition))
                {
                    continue;
                }

                GameObject pickupPrefab = GetRandomPickup();

                if (pickupPrefab == null)
                {
                    return;
                }

                Instantiate(
                    pickupPrefab,
                    spawnPosition,
                    Quaternion.identity,
                    transform
                );

                _spawnedPositions.Add(spawnPosition);
                return;
            }
        }

        private Vector2 GetRandomPosition()
        {
            float randomX = Random.Range(
                _minimumWorldPosition.x,
                _maximumWorldPosition.x
            );

            float randomY = Random.Range(
                _minimumWorldPosition.y,
                _maximumWorldPosition.y
            );

            return new Vector2(randomX, randomY);
        }

        private bool IsPositionValid(Vector2 position)
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
                float distance = Vector2.Distance(
                    position,
                    spawnedPosition
                );

                if (distance < _minimumSpawnDistance)
                {
                    return false;
                }
            }

            return true;
        }

        private GameObject GetRandomPickup()
        {
            bool spawnWeapon = Random.value < _weaponSpawnChance;

            if (spawnWeapon)
            {
                return GetRandomFromList(_weaponPickups);
            }

            return GetRandomFromList(_ammoPickups);
        }

        private GameObject GetRandomFromList(
            List<GameObject> pickupPrefabs
        )
        {
            if (pickupPrefabs == null || pickupPrefabs.Count == 0)
            {
                return null;
            }

            int randomIndex = Random.Range(
                0,
                pickupPrefabs.Count
            );

            return pickupPrefabs[randomIndex];
        }
    }
}