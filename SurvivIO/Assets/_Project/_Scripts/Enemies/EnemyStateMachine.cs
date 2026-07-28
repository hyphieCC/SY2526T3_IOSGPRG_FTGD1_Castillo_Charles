using UnityEngine;
using Castillo.Combat;

namespace Castillo.Enemies
{
    public class EnemyStateMachine : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyMovement _movement;
        [SerializeField] private EnemyDetection _detection;
        [SerializeField] private EnemyWeaponController _weaponController;

        [Header("Patrol")]
        [SerializeField] private float _patrolRadius = 5f;
        [SerializeField] private float _maximumPatrolDuration = 4f;

        public EnemyState CurrentState { get; private set; }

        private Vector2 _patrolOrigin;
        private float _patrolTimer;

        private void Awake()
        {
            _patrolOrigin = transform.position;
        }

        private void OnEnable()
        {
            _detection.TargetDetected += EnterSeekState;
            _detection.TargetLost += EnterPatrolState;
        }

        private void OnDisable()
        {
            _detection.TargetDetected -= EnterSeekState;
            _detection.TargetLost -= EnterPatrolState;
        }

        private void Start()
        {
            EnterPatrolState();
        }

        private void Update()
        {
            switch (CurrentState)
            {
                case EnemyState.Patrol:
                    {
                        UpdatePatrolState();
                        break;
                    }

                case EnemyState.Seek:
                    {
                        UpdateSeekState();
                        break;
                    }
            }
        }

        private void EnterPatrolState()
        {
            CurrentState = EnemyState.Patrol;
            SelectPatrolDestination();
        }

        private void EnterSeekState(Health target)
        {
            if (target == null)
            {
                return;
            }

            CurrentState = EnemyState.Seek;
        }

        private void UpdatePatrolState()
        {
            _patrolTimer += Time.deltaTime;

            if (_movement.HasReachedDestination ||
                _patrolTimer >= _maximumPatrolDuration)
            {
                SelectPatrolDestination();
            }
        }

        private void UpdateSeekState()
        {
            Health target = _detection.CurrentTarget;

            if (target == null || target.IsDead)
            {
                EnterPatrolState();
                return;
            }

            Vector2 targetPosition = target.transform.position;

            _movement.SetDestination(targetPosition);
            _weaponController.AimAt(targetPosition);
        }

        private void SelectPatrolDestination()
        {
            Vector2 randomOffset = Random.insideUnitCircle * _patrolRadius;
            Vector2 destination = _patrolOrigin + randomOffset;

            _movement.SetDestination(destination);
            _patrolTimer = 0f;
        }
    }
}