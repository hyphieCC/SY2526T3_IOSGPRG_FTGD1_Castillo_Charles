using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Castillo.Combat;

namespace Castillo.UI
{
    public class EnemyHealthBarUI : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Health _health;

        [Header("Display")]
        [SerializeField] private GameObject _healthBarCanvas;
        [SerializeField] private Image _healthBarFill;

        [Header("Visibility")]
        [SerializeField] private float _visibleDuration = 5f;

        private Coroutine _hideCoroutine;

        private void Awake()
        {
            _healthBarCanvas.SetActive(false);
            RefreshHealthBar(_health.CurrentHealth, _health.MaximumHealth);
        }

        private void OnEnable()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _health.HealthChanged -= OnHealthChanged;

            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }
        }

        private void OnHealthChanged(float currentHealth, float maximumHealth)
        {
            RefreshHealthBar(currentHealth, maximumHealth);

            if (currentHealth <= 0f)
            {
                return;
            }

            ShowHealthBar();
        }

        private void RefreshHealthBar(float currentHealth, float maximumHealth)
        {
            if (maximumHealth <= 0f)
            {
                _healthBarFill.fillAmount = 0f;
                return;
            }

            _healthBarFill.fillAmount = currentHealth / maximumHealth;
        }

        private void ShowHealthBar()
        {
            _healthBarCanvas.SetActive(true);

            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
            }

            _hideCoroutine = StartCoroutine(CO_HideHealthBar());
        }

        private IEnumerator CO_HideHealthBar()
        {
            yield return new WaitForSeconds(_visibleDuration);

            _healthBarCanvas.SetActive(false);
            _hideCoroutine = null;
        }
    }
}