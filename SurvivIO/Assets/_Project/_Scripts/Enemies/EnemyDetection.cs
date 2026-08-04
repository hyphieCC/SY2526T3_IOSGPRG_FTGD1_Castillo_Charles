using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Castillo.Combat;


namespace Castillo.Enemies
{
    public class EnemyDetection : MonoBehaviour
    {
        [SerializeField] private float _startupGracePeriod = 2f;
        [SerializeField] private CircleCollider2D _detectionCollider;

        private readonly List<Health> _detectedTargets = new List<Health>();

        public Health CurrentTarget { get; private set; }

        public event Action<Health> TargetDetected;
        public event Action TargetLost;

        private bool _canDetect;
        private Health _ownerHealth;

        private void Awake()
        {
            _ownerHealth = GetComponentInParent<Health>();
        }

        private void OnDisable()
        {
            UnsubscribeFromCurrentTarget();

            CurrentTarget = null;
            _detectedTargets.Clear();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_canDetect)
            {
                return;
            }

            Health detectedHealth = other.GetComponentInParent<Health>();

            if (detectedHealth == null)
            {
                return;
            }

            if (detectedHealth == _ownerHealth)
            {
                return;
            }

            if (detectedHealth.IsDead)
            {
                return;
            }

            if (_detectedTargets.Contains(detectedHealth))
            {
                return;
            }

            _detectedTargets.Add(detectedHealth);

            SelectNearestTarget();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Health detectedHealth = other.GetComponentInParent<Health>();

            if (detectedHealth == null)
            {
                return;
            }

            if (!_detectedTargets.Remove(detectedHealth))
            {
                return;
            }

            if (CurrentTarget != detectedHealth)
            {
                return;
            }

            SelectNextTarget();
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_startupGracePeriod);

            _canDetect = true;

            ScanForTargets();

        }

        private void ScanForTargets()
        {
            Vector2 center = _detectionCollider.transform.TransformPoint(
                    _detectionCollider.offset
                );

            float radius =
                _detectionCollider.radius * Mathf.Max(
                    _detectionCollider.transform.lossyScale.x,
                    _detectionCollider.transform.lossyScale.y
                );

            Collider2D[] colliders = Physics2D.OverlapCircleAll(center, radius);

            foreach (Collider2D detectedCollider in colliders)
            {
                Health detectedHealth = detectedCollider.GetComponentInParent<Health>();

                if (detectedHealth == null)
                {
                    continue;
                }

                if (detectedHealth == _ownerHealth)
                {
                    continue;
                }

                if (detectedHealth.IsDead)
                {
                    continue;
                }

                if (_detectedTargets.Contains(detectedHealth))
                {
                    continue;
                }

                _detectedTargets.Add(detectedHealth);
            }

            SelectNearestTarget();
        }

        private void SelectNearestTarget()
        {
            Health nearestTarget = GetNearestTarget();

            if (nearestTarget == null || nearestTarget == CurrentTarget)
            {
                return;
            }

            SetTarget(nearestTarget);
        }

        private Health GetNearestTarget()
        {
            Health nearestTarget = null;
            float nearestDistanceSquared = float.MaxValue;
            Vector2 ownerPosition = _ownerHealth.transform.position;

            foreach (Health detectedTarget in _detectedTargets)
            {
                if (detectedTarget == null ||
                    detectedTarget.IsDead ||
                    detectedTarget == _ownerHealth)
                {
                    continue;
                }

                Vector2 targetPosition = detectedTarget.transform.position;
                float distanceSquared = (targetPosition - ownerPosition).sqrMagnitude;

                if (distanceSquared >= nearestDistanceSquared)
                {
                    continue;
                }

                nearestDistanceSquared = distanceSquared;
                nearestTarget = detectedTarget;
            }

            return nearestTarget;
        }

        private void SetTarget(Health target)
        {
            if (target == CurrentTarget)
            {
                return;
            }

            UnsubscribeFromCurrentTarget();

            CurrentTarget = target;

            if (CurrentTarget == null)
            {
                TargetLost?.Invoke();
                return;
            }

            CurrentTarget.Died += OnCurrentTargetDied;
            TargetDetected?.Invoke(CurrentTarget);
        }

        private void SelectNextTarget()
        {
            UnsubscribeFromCurrentTarget();
            RemoveInvalidTargets();

            Health nearestTarget = GetNearestTarget();

            if (nearestTarget != null)
            {
                SetTarget(nearestTarget);
                return;
            }

            CurrentTarget = null;
            TargetLost?.Invoke();
        }

        private void RemoveInvalidTargets()
        {
            _detectedTargets.RemoveAll(
                target => target == null || target.IsDead
            );
        }

        private void OnCurrentTargetDied()
        {
            if (CurrentTarget != null)
            {
                _detectedTargets.Remove(CurrentTarget);
            }

            SelectNextTarget();
        }

        private void UnsubscribeFromCurrentTarget()
        {
            if (CurrentTarget == null)
            {
                return;
            }

            CurrentTarget.Died -= OnCurrentTargetDied;
        }

    }
}