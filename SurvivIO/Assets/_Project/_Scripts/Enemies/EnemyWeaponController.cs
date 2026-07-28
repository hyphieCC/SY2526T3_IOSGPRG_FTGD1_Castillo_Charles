using System;
using System.Collections.Generic;
using UnityEngine;
using Castillo.Loot;
using Castillo.Weapons;

namespace Castillo.Enemies
{
    public class EnemyWeaponController : MonoBehaviour
    {
        [SerializeField] private AmmoInventory _ammoInventory;
        [SerializeField] private Transform _aimPivot;
        [SerializeField] private Transform _weaponHolder;

        [Header("Available Weapons")]
        [SerializeField] private List<WeaponBase> _weaponPrefabs;

        public WeaponBase EquippedWeapon { get; private set; }

        public event Action<WeaponBase> WeaponEquipped;

        private void Start()
        {
            EquipRandomWeapon();
        }

        public void EquipRandomWeapon()
        {
            if (_weaponPrefabs == null || _weaponPrefabs.Count == 0)
            {
                Debug.LogWarning(
                    $"{nameof(EnemyWeaponController)} on {gameObject.name} " +
                    "has no weapon prefabs assigned."
                );

                return;
            }

            if (_weaponHolder == null)
            {
                Debug.LogError(
                    $"{nameof(EnemyWeaponController)} on {gameObject.name} " +
                    "is missing its weapon holder."
                );

                return;
            }

            WeaponBase weaponPrefab = GetRandomWeaponPrefab();

            if (weaponPrefab == null)
            {
                return;
            }

            EquippedWeapon = Instantiate(
                weaponPrefab,
                _weaponHolder.position,
                _weaponHolder.rotation,
                _weaponHolder
            );

            EquippedWeapon.transform.localPosition = Vector3.zero;
            EquippedWeapon.transform.localRotation = Quaternion.identity;

            EquippedWeapon.Initialize(_ammoInventory);
            WeaponEquipped?.Invoke(EquippedWeapon);
        }

        public void AimAt(Vector2 targetPosition)
        {
            if (_aimPivot == null)
            {
                return;
            }

            Vector2 direction = targetPosition - (Vector2)_aimPivot.position;

            if (direction.sqrMagnitude <= 0.001f)
            {
                return;
            }

            float angle = Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;

            _aimPivot.rotation = Quaternion.Euler(
                0f,
                0f,
                angle - 90f
            );
        }

        private WeaponBase GetRandomWeaponPrefab()
        {
            int randomIndex = UnityEngine.Random.Range(
                0,
                _weaponPrefabs.Count
            );

            return _weaponPrefabs[randomIndex];
        }
    }
}