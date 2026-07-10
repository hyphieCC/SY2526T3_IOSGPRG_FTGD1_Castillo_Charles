using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Castillo.Combat;

namespace Castillo.UI
{
    public class UI_HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private Slider _healthSlider;

        private void Start()
        {
            InitializeHealthBar();
        }

        private void OnEnable()
        {
            _health.HealthChanged += UpdateHealthBar;
        }

        private void OnDisable()
        {
            _health.HealthChanged -= UpdateHealthBar;
        }

        private void InitializeHealthBar()
        {
            _healthSlider.minValue = 0f;
            _healthSlider.maxValue = _health.MaximumHealth;

            UpdateHealthBar(_health.CurrentHealth, _health.MaximumHealth);
        }

        private void UpdateHealthBar(float currentHealth, float maximumHealth)
        {
            _healthSlider.value = currentHealth;
        }
    }
}