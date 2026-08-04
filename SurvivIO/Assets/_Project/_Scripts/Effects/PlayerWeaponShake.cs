using UnityEngine;
using Castillo.Weapons;

namespace Castillo.Effects
{
    public class PlayerWeaponShake : MonoBehaviour
    {
        [SerializeField] private WeaponInventory _weaponInventory;
        [SerializeField] private CameraShake _cameraShake;

        private WeaponBase _subscribedWeapon;

        private void OnEnable()
        {
            _weaponInventory.WeaponEquipped += OnWeaponEquipped;

            OnWeaponEquipped(_weaponInventory.EquippedWeapon);
        }

        private void OnDisable()
        {
            _weaponInventory.WeaponEquipped -= OnWeaponEquipped;

            UnsubscribeFromWeapon();
        }

        private void OnWeaponEquipped(WeaponBase weapon)
        {
            UnsubscribeFromWeapon();

            _subscribedWeapon = weapon;

            if (_subscribedWeapon == null)
            {
                return;
            }

            _subscribedWeapon.Fired += OnWeaponFired;
        }

        private void OnWeaponFired(WeaponBase weapon)
        {
            _cameraShake.Shake();
        }

        private void UnsubscribeFromWeapon()
        {
            if (_subscribedWeapon == null)
            {
                return;
            }

            _subscribedWeapon.Fired -= OnWeaponFired;
            _subscribedWeapon = null;
        }
    }
}