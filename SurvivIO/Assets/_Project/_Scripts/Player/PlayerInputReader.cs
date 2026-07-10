using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Castillo.Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;

        public Vector2 MoveInput { get; private set; }
        public Vector2 AimInput { get; private set; }

        public event Action FirePressed;
        public event Action PrimaryWeaponSelected;
        public event Action SecondaryWeaponSelected;

        private InputAction _moveAction;
        private InputAction _aimAction;
        private InputAction _fireAction;
        private InputAction _switchPrimaryAction;
        private InputAction _switchSecondaryAction;

        private void Awake()
        {
            _moveAction = _playerInput.actions["Move"];
            _aimAction = _playerInput.actions["Aim"];
            _fireAction = _playerInput.actions["Fire"];
            _switchPrimaryAction = _playerInput.actions["SwitchPrimary"];
            _switchSecondaryAction = _playerInput.actions["SwitchSecondary"];
        }

        private void OnEnable()
        {
            _moveAction.performed += OnMoveChanged;
            _moveAction.canceled += OnMoveChanged;

            _aimAction.performed += OnAimChanged;
            _aimAction.canceled += OnAimChanged;

            _fireAction.started += OnFireStarted;
            _switchPrimaryAction.started += OnSwitchPrimaryStarted;
            _switchSecondaryAction.started += OnSwitchSecondaryStarted;
        }

        private void OnDisable()
        {
            _moveAction.performed -= OnMoveChanged;
            _moveAction.canceled -= OnMoveChanged;

            _aimAction.performed -= OnAimChanged;
            _aimAction.canceled -= OnAimChanged;

            _fireAction.started -= OnFireStarted;
            _switchPrimaryAction.started -= OnSwitchPrimaryStarted;
            _switchSecondaryAction.started -= OnSwitchSecondaryStarted;
        }

        private void OnMoveChanged(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        private void OnAimChanged(InputAction.CallbackContext context)
        {
            AimInput = context.ReadValue<Vector2>();
        }

        private void OnFireStarted(InputAction.CallbackContext context)
        {
            FirePressed?.Invoke();
        }

        private void OnSwitchPrimaryStarted(InputAction.CallbackContext context)
        {
            PrimaryWeaponSelected?.Invoke();
        }

        private void OnSwitchSecondaryStarted(InputAction.CallbackContext context)
        {
            SecondaryWeaponSelected?.Invoke();
        }
    }
}