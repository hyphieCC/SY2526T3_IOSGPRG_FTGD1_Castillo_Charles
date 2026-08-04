using System.Collections;
using UnityEngine;
using Castillo.UI;

namespace Castillo.Effects
{
    public class HitPause : MonoBehaviour
    {
        public static HitPause Instance { get; private set; }

        [SerializeField] private float _pauseDuration = 0.1f;
        [SerializeField] private UI_GameResult _gameResultUI;

        private Coroutine _pauseCoroutine;

        private void Awake()
        {
            Instance = this;
        }

        public void Pause()
        {
            if (_gameResultUI != null && _gameResultUI.GameEnded)
            {
                return;
            }

            if (_pauseCoroutine != null)
            {
                StopCoroutine(_pauseCoroutine);
            }

            _pauseCoroutine = StartCoroutine(CO_Pause());
        }

        private IEnumerator CO_Pause()
        {
            Time.timeScale = 0f;

            yield return new WaitForSecondsRealtime(_pauseDuration);

            if (_gameResultUI == null || !_gameResultUI.GameEnded)
            {
                Time.timeScale = 1f;
            }

            _pauseCoroutine = null;
        }
    }
}