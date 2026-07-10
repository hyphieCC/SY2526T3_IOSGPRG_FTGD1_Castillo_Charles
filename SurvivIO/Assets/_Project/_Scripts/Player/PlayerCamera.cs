using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Castillo.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _followSpeed = 8f;

        private Vector3 _offset;

        private void Awake()
        {
            _offset = transform.position - _target.position;
        }

        private void LateUpdate()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            Vector3 targetPosition = _target.position + _offset;
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                _followSpeed * Time.deltaTime
            );
        }
    }
}