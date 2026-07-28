using UnityEngine;

namespace Castillo.Player
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader _inputReader;
        [SerializeField] private Transform _aimPivot;
        [SerializeField] private Transform _playerVisual;
        [SerializeField] private float _minimumAimMagnitude = 0.1f;

        public Vector2 AimDirection { get; private set; } = Vector2.up;

        private void Update()
        {
            Aim();
        }

        private void Aim()
        {
            Vector2 aimInput = _inputReader.AimInput;

            if (aimInput.sqrMagnitude < _minimumAimMagnitude * _minimumAimMagnitude)
            {
                return;
            }

            AimDirection = aimInput.normalized;

            float aimAngle = Mathf.Atan2(
                AimDirection.y,
                AimDirection.x
            ) * Mathf.Rad2Deg;

            _aimPivot.rotation = Quaternion.Euler(
                0f,
                0f,
                aimAngle - 90f
            );

            if (_playerVisual != null)
            {
                _playerVisual.rotation = _aimPivot.rotation;
            }
        }
    }
}