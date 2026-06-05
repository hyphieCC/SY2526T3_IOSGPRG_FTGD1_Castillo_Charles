using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private TMP_Text _scoreText;

    [Header("Dash Mechanic")]
    [SerializeField] private Slider _dashGaugeSlider;
    [SerializeField] private GameObject _dashButton;

    private int _currentScore;

    private void Start()
    {
        _gameOverPanel.SetActive(false);
        _dashButton.SetActive(false);
        UpdateDashGauge(0f, 100f);
    }

    public void ShowGameOver()
    {
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void BTN_Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateLivesText(int lives)
    {
        _livesText.text = $"Lives: {lives}";
    }

    public void UpdateDashGauge(float currentGauge, float maxGauge)
    {
        _dashGaugeSlider.value = currentGauge / maxGauge;

        if (_dashButton != null)
        {
            _dashButton.SetActive(currentGauge >= maxGauge);
        }
    }

    public void AddScore(int scoreAmount)
    {
        _currentScore += scoreAmount;
        _scoreText.text = $"Score: {_currentScore}";
    }

    public int CurrentScore
    {
        get => _currentScore;
    }
}
