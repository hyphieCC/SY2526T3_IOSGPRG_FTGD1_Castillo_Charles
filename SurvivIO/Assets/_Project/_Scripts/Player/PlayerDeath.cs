using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Castillo.Combat;
using Castillo.Weapons;

namespace Castillo.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Health _health;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAim _playerAim;
        [SerializeField] private WeaponInventory _weaponInventory;

        [Header("Collision")]
        [SerializeField] private Collider2D _playerCollider;

        [Header("Visual")]
        [SerializeField] private GameObject _playerVisual;
        [SerializeField] private GameObject _aimPivot;

        public bool IsDead { get; private set; }

        public event Action PlayerDied;

        private void OnEnable()
        {
            _health.Died += Die;
        }

        private void OnDisable()
        {
            _health.Died -= Die;
        }

        private void Die()
        {
            if (IsDead)
            {
                return;
            }

            IsDead = true;

            DisablePlayerControl();

            PlayerDied?.Invoke();
        }

        private void DisablePlayerControl()
        {
            if (_playerInput != null)
            {
                _playerInput.DeactivateInput();
            }

            if (_playerMovement != null)
            {
                _playerMovement.enabled = false;
            }

            if (_playerAim != null)
            {
                _playerAim.enabled = false;
            }

            if (_weaponInventory != null)
            {
                _weaponInventory.enabled = false;
            }

            if (_playerCollider != null)
            {
                _playerCollider.enabled = false;
            }

            if (_playerVisual != null)
            {
                _playerVisual.SetActive(false);
            }

            if (_aimPivot != null)
            {
                _aimPivot.SetActive(false);
            }
        }
    }
}