using System;
using System.Collections;
using UnityEngine;
using Castillo.Loot;

namespace Castillo.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Weapon Information")]
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private WeaponSlot _weaponSlot;
        [SerializeField] private AmmoType _ammoType;
        [SerializeField] private Sprite _weaponSlotIcon;

        [Header("Ammo")]
        [SerializeField] private int _clipCapacity;
        [SerializeField] private int _startingClipAmmo;

        [Header("Timing")]
        [SerializeField] private float _fireDelay;
        [SerializeField] private float _reloadDuration;

        [Header("Projectile")]
        [SerializeField] protected Transform _firePoint;
        [SerializeField] protected Projectile _projectilePrefab;

        public WeaponType WeaponType => _weaponType;
        public WeaponSlot WeaponSlot => _weaponSlot;
        public AmmoType AmmoType => _ammoType;
        public Sprite WeaponSlotIcon => _weaponSlotIcon;
        public int CurrentClipAmmo { get; private set; }
        public int ClipCapacity => _clipCapacity;
        public bool IsReloading { get; private set; }

        public event Action<WeaponBase> Fired;
        public event Action<WeaponBase> AmmoStateChanged;
        public event Action<WeaponBase> ReloadStarted;
        public event Action<WeaponBase> ReloadCompleted;

        private AmmoInventory _ammoInventory;
        private bool _hasUnlimitedAmmo;
        private float _reloadDurationMultiplier = 1f;
        private float _nextFireTime;
        private GameObject _owner;

        protected GameObject Owner => _owner;

        protected virtual void Awake()
        {
            CurrentClipAmmo = Mathf.Clamp(
                _startingClipAmmo,
                0,
                _clipCapacity
            );
        }

        protected virtual void OnDisable()
        {
            if (!IsReloading)
            {
                return;
            }

            StopAllCoroutines();
            IsReloading = false;
            ReloadCompleted?.Invoke(this);
        }

        public void Initialize(
            AmmoInventory ammoInventory,
            GameObject owner,
            bool hasUnlimitedAmmo = false,
            float reloadDurationMultiplier = 1f
        )
        {
            _ammoInventory = ammoInventory;
            _owner = owner;
            _hasUnlimitedAmmo = hasUnlimitedAmmo;

            _reloadDurationMultiplier = Mathf.Max(
                reloadDurationMultiplier,
                0f
            );

            AmmoStateChanged?.Invoke(this);
        }

        public void TryFire()
        {
            if (!CanFire())
            {
                return;
            }

            FireProjectile();

            CurrentClipAmmo--;
            _nextFireTime = Time.time + _fireDelay;

            Fired?.Invoke(this);
            AmmoStateChanged?.Invoke(this);

            if (CurrentClipAmmo <= 0)
            {
                TryReload();
            }
        }

        public void TryReload()
        {
            if (!CanReload())
            {
                return;
            }

            StartCoroutine(CO_Reload());
        }

        public virtual void BeginFire()
        {
            TryFire();
        }

        public virtual void EndFire()
        {
        }

        protected abstract void FireProjectile();

        private bool CanFire()
        {
            if (IsReloading)
            {
                return false;
            }

            if (Time.time < _nextFireTime)
            {
                return false;
            }

            if (CurrentClipAmmo <= 0)
            {
                TryReload();
                return false;
            }

            return true;
        }

        private bool CanReload()
        {
            if (IsReloading)
            {
                return false;
            }

            if (CurrentClipAmmo >= _clipCapacity)
            {
                return false;
            }

            if (_hasUnlimitedAmmo)
            {
                return true;
            }

            if (_ammoInventory == null)
            {
                return false;
            }

            if (_ammoInventory.GetAmmoAmount(_ammoType) <= 0)
            {
                return false;
            }

            return true;
        }

        private IEnumerator CO_Reload()
        {
            IsReloading = true;
            ReloadStarted?.Invoke(this);

            float adjustedReloadDuration = _reloadDuration * _reloadDurationMultiplier;

            yield return new WaitForSeconds(adjustedReloadDuration);

            int missingAmmo = _clipCapacity - CurrentClipAmmo;

            if (_hasUnlimitedAmmo)
            {
                CurrentClipAmmo += missingAmmo;
            }
            else
            {
                int ammoRemoved = _ammoInventory.RemoveAmmo(
                    _ammoType,
                    missingAmmo
                );

                CurrentClipAmmo += ammoRemoved;
            }

            IsReloading = false;

            ReloadCompleted?.Invoke(this);
            AmmoStateChanged?.Invoke(this);
        }
    }
}