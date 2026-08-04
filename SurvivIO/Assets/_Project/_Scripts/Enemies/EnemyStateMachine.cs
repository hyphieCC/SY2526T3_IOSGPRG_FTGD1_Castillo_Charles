using UnityEngine;
using Castillo.Combat;
using Castillo.Weapons;

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

        [Header("Attack")]
        [SerializeField] private float _attackDistance = 4f;
        [SerializeField] private float _semiAutomaticInputInterval = 0.1f;

        public EnemyState CurrentState { get; private set; }

        private Vector2 _patrolOrigin;
        private float _patrolTimer;
        private float _nextSemiAutomaticInputTime;

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

                case EnemyState.Attack:
                    {
                        UpdateAttackState();
                        break;
                    }
            }
        }

        private void EnterPatrolState()
        {
            ExitAttackState();
            CurrentState = EnemyState.Patrol;
            SelectPatrolDestination();
        }

        private void EnterSeekState(Health target)
        {
            if (target == null)
            {
                return;
            }

            ExitAttackState();
            CurrentState = EnemyState.Seek;
        }

        private void EnterAttackState()
        {
            CurrentState = EnemyState.Attack;
            _movement.Stop();
            _nextSemiAutomaticInputTime = 0f;
        }

        private void UpdatePatrolState()
        {
            _patrolTimer += Time.deltaTime;

            if (_movement.HasReachedDestination || _patrolTimer >= _maximumPatrolDuration)
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
            float distanceSquared = ((Vector2)transform.position - targetPosition).sqrMagnitude;

            _weaponController.AimAt(targetPosition);

            if (distanceSquared <= _attackDistance * _attackDistance)
            {
                EnterAttackState();
                return;
            }

            _movement.SetDestination(targetPosition);
        }

        private void UpdateAttackState()
        {
            Health target = _detection.CurrentTarget;

            if (target == null || target.IsDead)
            {
                ExitAttackState();
                EnterPatrolState();
                return;
            }

            Vector2 targetPosition = target.transform.position;
            float distanceSquared =
                ((Vector2)transform.position - targetPosition).sqrMagnitude;

            if (distanceSquared > _attackDistance * _attackDistance)
            {
                ExitAttackState();
                EnterSeekState(target);
                return;
            }

            _weaponController.AimAt(targetPosition);
            FireAtTarget();
        }

        private void ExitAttackState()
        {
            _weaponController.EndFiring();
        }

        private void SelectPatrolDestination()
        {
            Vector2 randomOffset = Random.insideUnitCircle * _patrolRadius;
            Vector2 destination = _patrolOrigin + randomOffset;

            _movement.SetDestination(destination);
            _patrolTimer = 0f;
        }

        private void FireAtTarget()
        {
            WeaponBase equippedWeapon = _weaponController.EquippedWeapon;

            if (equippedWeapon == null)
            {
                return;
            }

            if (equippedWeapon.WeaponType == WeaponType.Rifle)
            {
                _weaponController.BeginFiring();
                return;
            }

            if (Time.time < _nextSemiAutomaticInputTime)
            {
                return;
            }

            _weaponController.BeginFiring();
            _nextSemiAutomaticInputTime = Time.time + _semiAutomaticInputInterval;
        }
    }
}