using UnityEngine;
using UnityEngine.SceneManagement;
using Castillo.Player;
using Castillo.Enemies;

namespace Castillo.UI
{
    public class UI_GameResult : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerDeath _playerDeath;
        [SerializeField] private EnemySpawner _enemySpawner;

        [Header("Panels")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _winPanel;

        [Header("Scenes")]
        [SerializeField] private string _mainMenuSceneName = "MainMenu";

        private bool _gameEnded;

        public bool GameEnded => _gameEnded;

        private void Awake()
        {
            _gameOverPanel.SetActive(false);
            _winPanel.SetActive(false);
        }

        private void OnEnable()
        {
            _playerDeath.PlayerDied += ShowGameOver;
            _enemySpawner.AllEnemiesDefeated += ShowChickenDinner;
        }

        private void OnDisable()
        {
            _playerDeath.PlayerDied -= ShowGameOver;
            _enemySpawner.AllEnemiesDefeated -= ShowChickenDinner;
        }

        private void ShowGameOver()
        {
            if (_gameEnded)
            {
                return;
            }

            _gameEnded = true;

            _gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        private void ShowChickenDinner()
        {
            if (_gameEnded)
            {
                return;
            }

            _gameEnded = true;

            _winPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Retry()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().name
            );
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(_mainMenuSceneName);
        }
    }
}