using TMPro;
using UnityEngine;
using Castillo.Enemies;
using Castillo.Player;

namespace Castillo.UI
{
    public class UI_EntityCount : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private PlayerDeath _playerDeath;
        [SerializeField] private TMP_Text _entityCountText;

        private bool _playerAlive = true;

        private void OnEnable()
        {
            _enemySpawner.AliveEnemyCountChanged += OnEnemyCountChanged;
            _playerDeath.PlayerDied += OnPlayerDied;
        }

        private void OnDisable()
        {
            _enemySpawner.AliveEnemyCountChanged -= OnEnemyCountChanged;
            _playerDeath.PlayerDied -= OnPlayerDied;
        }

        private void Start()
        {
            RefreshDisplay();
        }

        private void OnEnemyCountChanged(int aliveEnemyCount)
        {
            RefreshDisplay();
        }

        private void OnPlayerDied()
        {
            _playerAlive = false;
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            int totalEntities =
                _enemySpawner.AliveEnemyCount +
                (_playerAlive ? 1 : 0);

            _entityCountText.text = $"{totalEntities} ALIVE";
        }
    }
}