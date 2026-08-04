using UnityEngine;
using UnityEngine.UI;
using Castillo.Weapons;

namespace Castillo.UI
{
    public class UI_WeaponSlotDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private WeaponInventory _weaponInventory;

        [Header("Slot Images")]
        [SerializeField] private Image _primaryWeaponImage;
        [SerializeField] private GameObject _primarySelectedBackground;
        [SerializeField] private Image _secondaryWeaponImage;
        [SerializeField] private GameObject _secondarySelectedBackground;

        [Header("Empty Slot")]
        [SerializeField] private Sprite _emptySlotSprite;

        private void OnEnable()
        {
            _weaponInventory.WeaponSlotChanged += OnWeaponSlotChanged;
            _weaponInventory.WeaponEquipped += OnWeaponEquipped;
        }

        private void OnDisable()
        {
            _weaponInventory.WeaponSlotChanged -= OnWeaponSlotChanged;
            _weaponInventory.WeaponEquipped -= OnWeaponEquipped;
        }

        private void Awake()
        {
            HideSelectedBackgrounds();
        }

        private void Start()
        {
            RefreshAllSlots();
            OnWeaponEquipped(_weaponInventory.EquippedWeapon);
        }

        private void OnWeaponSlotChanged(WeaponSlot weaponSlot, WeaponBase weapon)
        {
            switch (weaponSlot)
            {
                case WeaponSlot.Primary:
                    {
                        UpdateSlotImage(_primaryWeaponImage, weapon);
                        break;
                    }

                case WeaponSlot.Secondary:
                    {
                        UpdateSlotImage(_secondaryWeaponImage, weapon);
                        break;
                    }
            }
        }

        private void OnWeaponEquipped(WeaponBase weapon)
        {
            if (weapon == null)
            {
                HideSelectedBackgrounds();
                return;
            }

            _primarySelectedBackground.SetActive(
                weapon.WeaponSlot == WeaponSlot.Primary
            );

            _secondarySelectedBackground.SetActive(
                weapon.WeaponSlot == WeaponSlot.Secondary
            );
        }

        private void RefreshAllSlots()
        {
            UpdateSlotImage(
                _primaryWeaponImage,
                _weaponInventory.PrimaryWeapon
            );

            UpdateSlotImage(
                _secondaryWeaponImage,
                _weaponInventory.SecondaryWeapon
            );
        }

        private void UpdateSlotImage(Image slotImage, WeaponBase weapon)
        {
            if (weapon == null)
            {
                slotImage.sprite = _emptySlotSprite;
                return;
            }

            slotImage.sprite = weapon.WeaponSlotIcon;
        }

        private void HideSelectedBackgrounds()
        {
            _primarySelectedBackground.SetActive(false);
            _secondarySelectedBackground.SetActive(false);
        }
    }
}