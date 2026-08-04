using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Castillo.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private PlayerInputReader _inputReader;
        [SerializeField] private float _moveSpeed = 5f;

        private float _speedMultiplier = 1f;

        private void FixedUpdate()
        {
            Move();
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            _speedMultiplier = Mathf.Max(0f, multiplier);
        }

        private void Move()
        {
            Vector2 movement = _inputReader.MoveInput.normalized;
            Vector2 targetPosition = _rigidbody.position + movement * _moveSpeed * Time.fixedDeltaTime
                * _speedMultiplier;

            _rigidbody.MovePosition(targetPosition);
        }
    }
}