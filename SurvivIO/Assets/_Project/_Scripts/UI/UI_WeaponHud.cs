using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Castillo.Loot;
using Castillo.Weapons;

namespace Castillo.UI
{
    public class UI_WeaponHud : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private WeaponInventory _weaponInventory;
        [SerializeField] private AmmoInventory _ammoInventory;

        [Header("UI")]
        [SerializeField] private TMP_Text _clipAmmoText;
        [SerializeField] private TMP_Text _reserveAmmoText;
        [SerializeField] private TMP_Text _reloadText;

        private WeaponBase _equippedWeapon;

        private void OnEnable()
        {
            _weaponInventory.WeaponEquipped += OnWeaponEquipped;
            _ammoInventory.AmmoChanged += OnReserveAmmoChanged;
        }

        private void OnDisable()
        {
            _weaponInventory.WeaponEquipped -= OnWeaponEquipped;
            _ammoInventory.AmmoChanged -= OnReserveAmmoChanged;

            UnsubscribeFromWeapon();
        }

        private void Start()
        {
            ClearWeaponDisplay();
        }

        private void OnWeaponEquipped(WeaponBase weapon)
        {
            UnsubscribeFromWeapon();

            _equippedWeapon = weapon;

            if (_equippedWeapon == null)
            {
                ClearWeaponDisplay();
                return;
            }

            _equippedWeapon.AmmoStateChanged += UpdateWeaponDisplay;
            _equippedWeapon.ReloadStarted += ShowReloading;
            _equippedWeapon.ReloadCompleted += HideReloading;

            UpdateWeaponDisplay(_equippedWeapon);
        }

        private void OnReserveAmmoChanged(AmmoType ammoType, int currentAmount)
        {
            if (_equippedWeapon == null)
            {
                return;
            }

            if (_equippedWeapon.AmmoType != ammoType)
            {
                return;
            }

            UpdateReserveAmmoText();
        }

        private void UpdateWeaponDisplay(WeaponBase weapon)
        {
            if (weapon != _equippedWeapon)
            {
                return;
            }

            _clipAmmoText.text = _equippedWeapon.CurrentClipAmmo.ToString();

            UpdateReserveAmmoText();
        }

        private void UpdateReserveAmmoText()
        {
            int reserveAmmo = _ammoInventory.GetAmmoAmount(
                _equippedWeapon.AmmoType
            );

            _reserveAmmoText.text = reserveAmmo.ToString();
        }

        private void ShowReloading(WeaponBase weapon)
        {
            if (weapon != _equippedWeapon)
            {
                return;
            }

            _reloadText.gameObject.SetActive(true);
        }

        private void HideReloading(WeaponBase weapon)
        {
            if (weapon != _equippedWeapon)
            {
                return;
            }

            _reloadText.gameObject.SetActive(false);
            UpdateWeaponDisplay(weapon);
        }

        private void UnsubscribeFromWeapon()
        {
            if (_equippedWeapon == null)
            {
                return;
            }

            _equippedWeapon.AmmoStateChanged -= UpdateWeaponDisplay;
            _equippedWeapon.ReloadStarted -= ShowReloading;
            _equippedWeapon.ReloadCompleted -= HideReloading;
        }

        private void ClearWeaponDisplay()
        {
            _equippedWeapon = null;

            _clipAmmoText.text = "0";
            _reserveAmmoText.text = "0";
            _reloadText.gameObject.SetActive(false);
        }
    }
}