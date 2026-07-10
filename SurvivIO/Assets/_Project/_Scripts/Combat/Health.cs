using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Castillo.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _maximumHealth = 100f;

        public float CurrentHealth { get; private set; }
        public float MaximumHealth => _maximumHealth;
        public bool IsDead => CurrentHealth <= 0f;

        public event Action<float, float> HealthChanged;
        public event Action Died;

        private void Awake()
        {
            CurrentHealth = _maximumHealth;
        }

        public void TakeDamage(float damageAmount)
        {
            if (damageAmount <= 0f || IsDead)
            {
                return;
            }

            CurrentHealth = Mathf.Max(CurrentHealth - damageAmount, 0f);
            HealthChanged?.Invoke(CurrentHealth, _maximumHealth);

            if (IsDead)
            {
                Died?.Invoke();
            }
        }

        public void Heal(float healAmount)
        {
            if (healAmount <= 0f || IsDead)
            {
                return;
            }

            CurrentHealth = Mathf.Min(CurrentHealth + healAmount, _maximumHealth);
            HealthChanged?.Invoke(CurrentHealth, _maximumHealth);
        }

        public void RestoreToMaximum()
        {
            CurrentHealth = _maximumHealth;
            HealthChanged?.Invoke(CurrentHealth, _maximumHealth);
        }
    }
}