using System;
using System.Collections.Generic;
using UnityEngine;
using Castillo.Combat;


namespace Castillo.Enemies
{
    public class EnemyDetection : MonoBehaviour
    {
        private readonly List<Health> _detectedTargets = new List<Health>();

        public Health CurrentTarget { get; private set; }

        public event Action<Health> TargetDetected;
        public event Action TargetLost;

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