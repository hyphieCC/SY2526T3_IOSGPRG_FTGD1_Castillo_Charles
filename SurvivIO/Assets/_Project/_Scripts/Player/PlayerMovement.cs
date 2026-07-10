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

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector2 movement = _inputReader.MoveInput.normalized;
            Vector2 targetPosition = _rigidbody.position + movement * _moveSpeed * Time.fixedDeltaTime;

            _rigidbody.MovePosition(targetPosition);
        }
    }
}