using UnityEngine;
using Castillo.Combat;

namespace Castillo.World
{
    public class DestructibleBarrel : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Health _health;

        [Header("Scale")]
        [SerializeField] private float _scaleReductionPerHit = 0.8f;
        [SerializeField] private float _minimumScale = 0.4f;

        [Header("Loot")]
        [SerializeField] private GameObject[] _gunPickupPrefabs;
        [SerializeField] private GameObject[] _ammoPickupPrefabs;
        [SerializeField] private GameObject _healthPickupPrefab;

        private void OnEnable()
        {
            _health.Damaged += Shrink;
            _health.Died += DestroyBarrel;
        }

        private void OnDisable()
        {
            _health.Damaged -= Shrink;
            _health.Died -= DestroyBarrel;
        }

        private void Shrink()
        {
            Vector3 newScale = transform.localScale * _scaleReductionPerHit;

            newScale.x = Mathf.Max(newScale.x, _minimumScale);
            newScale.y = Mathf.Max(newScale.y, _minimumScale);

            transform.localScale = newScale;
        }

        private void DestroyBarrel()
        {
            DropLoot();
            Destroy(gameObject);
        }

        private void DropLoot()
        {
            float randomValue = Random.value;

            if (randomValue < 0.25f)
            {
                SpawnRandomLoot(_gunPickupPrefabs);
                return;
            }

            if (randomValue < 0.50f)
            {
                SpawnHealthPickup();
                return;
            }

            SpawnRandomLoot(_ammoPickupPrefabs);
        }

        private void SpawnRandomLoot(GameObject[] lootPrefabs)
        {
            if (lootPrefabs == null || lootPrefabs.Length == 0)
            {
                return;
            }

            int randomIndex = Random.Range(0, lootPrefabs.Length);

            Instantiate(
                lootPrefabs[randomIndex],
                transform.position,
                Quaternion.identity
            );
        }

        private void SpawnHealthPickup()
        {
            if (_healthPickupPrefab == null)
            {
                return;
            }

            Instantiate(
                _healthPickupPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}