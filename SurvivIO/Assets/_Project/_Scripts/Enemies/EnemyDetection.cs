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

        private void OnDisable()
        {
            UnsubscribeFromCurrentTarget();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Health health))
            {
                return;
            }

            if (health.transform.root == transform.root)
            {
                return;
            }

            if (_detectedTargets.Contains(health))
            {
                return;
            }

            _detectedTargets.Add(health);

            if (CurrentTarget == null)
            {
                SetTarget(health);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Health health))
            {
                return;
            }

            if (!_detectedTargets.Remove(health))
            {
                return;
            }

            if (CurrentTarget != health)
            {
                return;
            }

            SelectNextTarget();
        }

        private void SetTarget(Health target)
        {
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

            if (_detectedTargets.Count > 0)
            {
                SetTarget(_detectedTargets[0]);
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