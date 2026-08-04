using System;
using System.Collections;
using UnityEngine;
using Castillo.Combat;
using Castillo.Weapons;
using Castillo.Player;

namespace Castillo.Loot
{
    public class HealthPickupUse : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HealthInventory _inventory;
        [SerializeField] private Health _health;
        [SerializeField] private WeaponInventory _weaponInventory;
        [SerializeField] private PlayerInputReader _inputReader;
        [SerializeField] private PlayerMovement _playerMovement;

        [Header("Movement")]
        [SerializeField] private float _healingSpeedMultiplier = 0.5f;

        [Header("Healing")]
        [SerializeField] private float _useDuration = 3f;
        [SerializeField] private float _healAmount = 30f;

        public bool IsUsingHealthPickup { get; private set; }

        public event Action<float, float> UseProgressChanged;
        public event Action<bool> UseStateChanged;

        private Coroutine _useCoroutine;

        private void OnEnable()
        {
            _inputReader.FirePressed += CancelUse;
        }

        private void OnDisable()
        {
            _inputReader.FirePressed -= CancelUse;

            CancelUse();
        }

        public void UseHealthPickup()
        {
            if (IsUsingHealthPickup)
            {
                return;
            }

            if (_inventory.CurrentAmount <= 0)
            {
                return;
            }

            if (_health.CurrentHealth >= _health.MaximumHealth)
            {
                return;
            }

            _useCoroutine = StartCoroutine(CO_UseHealthPickup());
        }

        public void CancelUse()
        {
            if (!IsUsingHealthPickup)
            {
                return;
            }

            if (_useCoroutine != null)
            {
                StopCoroutine(_useCoroutine);
                _useCoroutine = null;
            }

            IsUsingHealthPickup = false;

            _playerMovement.SetSpeedMultiplier(1f);

            UseProgressChanged?.Invoke(0f, _useDuration);
            UseStateChanged?.Invoke(false);
        }

        private IEnumerator CO_UseHealthPickup()
        {
            IsUsingHealthPickup = true;

            _playerMovement.SetSpeedMultiplier(
                _healingSpeedMultiplier
            );

            UseStateChanged?.Invoke(true);

            float elapsedTime = 0f;

            while (elapsedTime < _useDuration)
            {
                elapsedTime += Time.deltaTime;

                UseProgressChanged?.Invoke(
                    elapsedTime,
                    _useDuration
                );

                yield return null;
            }

            if (_inventory.TryConsumeHealthPickup())
            {
                _health.Heal(_healAmount);
            }

            FinishUse();
        }

        private void FinishUse()
        {
            IsUsingHealthPickup = false;

            _playerMovement.SetSpeedMultiplier(1f);

            UseProgressChanged?.Invoke(0f, _useDuration);
            UseStateChanged?.Invoke(false);

            _useCoroutine = null;
        }
    }
}