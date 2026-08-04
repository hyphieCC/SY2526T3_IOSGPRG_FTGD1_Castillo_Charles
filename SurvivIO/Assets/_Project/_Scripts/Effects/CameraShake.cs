using System.Collections;
using UnityEngine;

namespace Castillo.Effects
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.08f;
        [SerializeField] private float _strength = 0.12f;

        private Vector3 _originalLocalPosition;
        private Coroutine _shakeCoroutine;

        public void Shake()
        {
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }

            _originalLocalPosition = transform.localPosition;

            _shakeCoroutine = StartCoroutine(CO_Shake());
        }

        private IEnumerator CO_Shake()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _duration)
            {
                Vector2 randomOffset = Random.insideUnitCircle * _strength;

                transform.localPosition = _originalLocalPosition +
                    new Vector3(
                        randomOffset.x,
                        randomOffset.y,
                        0f
                    );

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = _originalLocalPosition;
            _shakeCoroutine = null;
        }
    }
}