using System;
using UnityEngine;
using Castillo.Loot;
using Castillo.Player;

namespace Castillo.Weapons
{
    public class WeaponInventory : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerInputReader _inputReader;
        [SerializeField] private AmmoInventory _ammoInventory;

        [Header("Weapon Holders")]
        [SerializeField] private Transform _primaryWeaponHolder;
        [SerializeField] private Transform _secondaryWeaponHolder;

        private WeaponBase _primaryWeapon;
        private WeaponBase _secondaryWeapon;

        public WeaponBase PrimaryWeapon => _primaryWeapon;
        public WeaponBase SecondaryWeapon => _secondaryWeapon;
        public WeaponBase EquippedWeapon { get; private set; }

        public event Action<WeaponBase> WeaponEquipped;
        public event Action<WeaponSlot, WeaponBase> WeaponSlotChanged;

        private void OnEnable()
        {
            _inputReader.FirePressed += BeginFiringEquippedWeapon;
            _inputReader.FireReleased += EndFiringEquippedWeapon;
            _inputReader.PrimaryWeaponSelected += EquipPrimaryWeapon;
            _inputReader.SecondaryWeaponSelected += EquipSecondaryWeapon;
        }

        private void OnDisable()
        {
            _inputReader.FirePressed -= BeginFiringEquippedWeapon;
            _inputReader.FireReleased -= EndFiringEquippedWeapon;
            _inputReader.PrimaryWeaponSelected -= EquipPrimaryWeapon;
            _inputReader.SecondaryWeaponSelected -= EquipSecondaryWeapon;
        }

        private void Awake()
        {
        }

        public void AddWeapon(WeaponBase weaponPrefab)
        {
            if (weaponPrefab == null)
            {
                return;
            }

            switch (weaponPrefab.WeaponSlot)
            {
                case WeaponSlot.Primary:
                    {
                        ReplaceWeapon(
                            ref _primaryWeapon,
                            weaponPrefab,
                            _primaryWeaponHolder
                        );

                        WeaponSlotChanged?.Invoke(
                            WeaponSlot.Primary,
                            _primaryWeapon
                        );

                        EquipPrimaryWeapon();
                        break;
                    }

                case WeaponSlot.Secondary:
                    {
                        ReplaceWeapon(
                            ref _secondaryWeapon,
                            weaponPrefab,
                            _secondaryWeaponHolder
                        );

                        WeaponSlotChanged?.Invoke(
                            WeaponSlot.Secondary,
                            _secondaryWeapon
                        );

                        EquipSecondaryWeapon();
                        break;
                    }
            }
        }

        public void EquipPrimaryWeapon()
        {
            if (_primaryWeapon == null)
            {
                return;
            }

            EquipWeapon(_primaryWeapon);
        }

        public void EquipSecondaryWeapon()
        {
            if (_secondaryWeapon == null)
            {
                return;
            }

            EquipWeapon(_secondaryWeapon);
        }

        public void ReloadEquippedWeapon()
        {
            if (EquippedWeapon == null)
            {
                return;
            }

            EquippedWeapon.TryReload();
        }

        private void ReplaceWeapon(
            ref WeaponBase currentWeapon,
            WeaponBase weaponPrefab,
            Transform holder)
        {
            if (weaponPrefab == null || holder == null)
            {
                return;
            }

            if (currentWeapon != null)
            {
                if (EquippedWeapon == currentWeapon)
                {
                    EquippedWeapon = null;
                }

                Destroy(currentWeapon.gameObject);
            }

            currentWeapon = Instantiate(
                weaponPrefab,
                holder.position,
                holder.rotation,
                holder
            );

            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;

            currentWeapon.Initialize(_ammoInventory);
        }

        private void EquipWeapon(WeaponBase weapon)
        {
            EquippedWeapon = weapon;

            if (_primaryWeapon != null)
            {
                _primaryWeapon.gameObject.SetActive(
                    _primaryWeapon == EquippedWeapon
                );
            }

            if (_secondaryWeapon != null)
            {
                _secondaryWeapon.gameObject.SetActive(
                    _secondaryWeapon == EquippedWeapon
                );
            }

            WeaponEquipped?.Invoke(EquippedWeapon);
        }

        private void BeginFiringEquippedWeapon()
        {
            if (EquippedWeapon == null)
            {
                return;
            }

            EquippedWeapon.BeginFire();
        }

        private void EndFiringEquippedWeapon()
        {
            if (EquippedWeapon == null)
            {
                return;
            }

            EquippedWeapon.EndFire();
        }
    }
}