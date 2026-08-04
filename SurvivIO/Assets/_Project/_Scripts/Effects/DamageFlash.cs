using System.Collections;
using UnityEngine;
using Castillo.Combat;

namespace Castillo.Effects
{
    public class DamageFlash : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _flashDuration = 0.08f;

        private Color _originalColor;
        private Coroutine _flashCoroutine;

        private void Awake()
        {
            _originalColor = _spriteRenderer.color;
        }

        private void OnEnable()
        {
            _health.Damaged += Flash;
        }

        private void OnDisable()
        {
            _health.Damaged -= Flash;

            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
                _flashCoroutine = null;
            }

            _spriteRenderer.color = _originalColor;
        }

        private void Flash()
        {
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
            }

            _flashCoroutine = StartCoroutine(CO_Flash());
        }

        private IEnumerator CO_Flash()
        {
            _spriteRenderer.color = Color.white;

            yield return new WaitForSecondsRealtime(_flashDuration);

            _spriteRenderer.color = _originalColor;
            _flashCoroutine = null;
        }
    }
}