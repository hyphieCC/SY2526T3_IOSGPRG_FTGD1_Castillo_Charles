using UnityEngine;
using Castillo.Weapons;

namespace Castillo.Loot
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private WeaponBase _weaponPrefab;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out WeaponInventory weaponInventory))
            {
                return;
            }

            if (_weaponPrefab == null)
            {
                return;
            }

            weaponInventory.AddWeapon(_weaponPrefab);
            Destroy(gameObject);
        }
    }
}