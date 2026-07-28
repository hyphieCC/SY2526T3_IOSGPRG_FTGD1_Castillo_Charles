using UnityEngine;

namespace Castillo.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _stoppingDistance = 0.1f;

        private Vector2 _destination;
        private bool _hasDestination;

        private void Awake()
        {
            _rigidbody.position = transform.position;
        }

        private void FixedUpdate()
        {
            MoveTowardsDestination();
        }

        public bool HasReachedDestination
        {
            get
            {
                if (!_hasDestination)
                {
                    return true;
                }

                float distanceSquared = (_destination - _rigidbody.position).sqrMagnitude;

                return distanceSquared <= _stoppingDistance * _stoppingDistance;
            }
        }

        public void SetDestination(Vector2 destination)
        {
            _destination = destination;
            _hasDestination = true;
        }

        public void Stop()
        {
            _hasDestination = false;
            _rigidbody.velocity = Vector2.zero;
        }

        private void MoveTowardsDestination()
        {
            if (!_hasDestination)
            {
                return;
            }

            Vector2 direction = (_destination - _rigidbody.position).normalized;
            Vector2 nextPosition = _rigidbody.position + direction * _moveSpeed * Time.fixedDeltaTime;

            _rigidbody.MovePosition(nextPosition);

            if (HasReachedDestination)
            {
                _hasDestination = false;
            }
        }
    }
}