using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private int _startHealth;
    [SerializeField] private GameStateUI _gameStateUI;
    [SerializeField] private int _killScoreGain;

    [Header("Visuals")]
    [SerializeField] private GameObject _defaultVisual;
    [SerializeField] private GameObject _tankVisual;
    [SerializeField] private GameObject _speedVisual;

    [Header("Dash Gauge")]
    [SerializeField] private float _maxDashGauge;
    [SerializeField] private float _dashGaugeGain;

    [Header("Dash")]
    [SerializeField] private float _dashDuration; //Would be (n/3) seconds in reality because of Time.timeScale = 3f during dash
    [SerializeField] private float _dashDrainSpeed;

    [Header("Tap Leap")]
    [SerializeField] private float _leapHeight;
    [SerializeField] private float _leapSpeed;
    [SerializeField] private int _tapLeapScoreGain;
    [SerializeField] private float _tapLeapDashGaugeGain;

    private bool _hasSelectedCharacter;
    private int _currentHealth;

    private float _currentDashGauge;
    private bool _isDashing;

    private Vector3 _startPosition;
    private bool _isTapLeaping;

    List<Enemy> _enemies = new List<Enemy>();

    private void Start()
    {
        _defaultVisual.SetActive(false);
        _tankVisual.SetActive(false);
        _speedVisual.SetActive(false);

        _startPosition = transform.position;
        _currentHealth = _startHealth;
        _gameStateUI.UpdateDashGauge(_currentDashGauge, _maxDashGauge);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            _enemies.Add(enemy);
            enemy.SetPlayerInRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            _enemies.Remove(enemy);
            enemy.SetPlayerInRange(false);
        }
    }

    public void SelectCharacter(CharacterType characterType)
    {
        _defaultVisual.SetActive(false);
        _tankVisual.SetActive(false);
        _speedVisual.SetActive(false);

        switch (characterType)
        {
            case CharacterType.Default:
                {
                    _defaultVisual.SetActive(true);
                    _startHealth = 3;
                    _dashGaugeGain = 5f;
                    break;
                }

            case CharacterType.Tank:
                {
                    _tankVisual.SetActive(true);
                    _startHealth = 5;
                    _dashGaugeGain = 5f;
                    break;
                }

            case CharacterType.Speed:
                {
                    _speedVisual.SetActive(true);
                    _startHealth = 3;
                    _dashGaugeGain = 10f;
                    break;
                }
        }

        _currentHealth = _startHealth;
        _gameStateUI.UpdateLivesText(_currentHealth);
        _hasSelectedCharacter = true;
    }

    public void CheckSwipe(SwipeDirection direction)
    {
        if (!_hasSelectedCharacter)
        {
            return;
        }

        if (_isDashing)
        {
            return;
        }

        if (_enemies.Count <= 0)
        {
            return;
        }

        Enemy enemy = _enemies[0];

        if (enemy.CheckPlayerSwipe(direction))
        {
            KillEnemy(enemy);
        }
        else
        {
            DamageFromEnemy(enemy);
        }
    }

    public void KillEnemy(Enemy enemy)
    {
        enemy.MarkAsInteractedWithPlayer();
        _enemies.Remove(enemy);

        _gameStateUI.AddScore(_killScoreGain);
        PowerupChance();

        if (!_isDashing)
        {
            AddDashGauge(_dashGaugeGain);
        }

        Destroy(enemy.gameObject);
    }

    public void DamageFromEnemy(Enemy enemy)
    {
        if (enemy.HasInteractedWithPlayer)
        {
            return;
        }

        enemy.MarkAsInteractedWithPlayer();
        TakeDamage(1);
    }

    public void BTN_Dash()
    {
        if (!_hasSelectedCharacter)
        {
            return;
        }

        if (_isDashing)
        {
            return;
        }

        if (_currentDashGauge < _maxDashGauge)
        {
            return;
        }

        StartCoroutine(CO_Dash());
    }

    public bool IsDashing
    {
        get => _isDashing;
    }

    public void TapLeap()
    {
        if (!_hasSelectedCharacter)
        {
            return;
        }

        if (_isDashing)
        {
            return;
        }

        if (_isTapLeaping)
        {
            return;
        }

        _gameStateUI.AddScore(_tapLeapScoreGain);
        AddDashGauge(_tapLeapDashGaugeGain);

        StartCoroutine(CO_TapLeap());
    }

    private void PowerupChance()
    {
        int randomChance = Random.Range(1, 101);

        if (randomChance <= 3)
        {
            _currentHealth++;
            _gameStateUI.UpdateLivesText(_currentHealth);
        }
    }

    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _gameStateUI.UpdateLivesText(_currentHealth);

        if (_currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        _gameStateUI.ShowGameOver();
    }

    private void AddDashGauge(float amount)
    {
        if (_isDashing)
        {
            return;
        }

        _currentDashGauge += amount;

        if (_currentDashGauge > _maxDashGauge)
        {
            _currentDashGauge = _maxDashGauge;
        }

        _gameStateUI.UpdateDashGauge(_currentDashGauge, _maxDashGauge);
    }

    private IEnumerator CO_Dash()
    {
        _isDashing = true;
        Time.timeScale = 3f;
        float currentDashTime = 0f;

        while (currentDashTime < _dashDuration)
        {
            currentDashTime += Time.deltaTime;
            _currentDashGauge -= _dashDrainSpeed * Time.deltaTime;

            if (_currentDashGauge < 0f)
            {
                _currentDashGauge = 0f;
            }

            _gameStateUI.UpdateDashGauge(_currentDashGauge, _maxDashGauge);

            yield return null;
        }

        _currentDashGauge = 0f;
        _gameStateUI.UpdateDashGauge(_currentDashGauge, _maxDashGauge);
        _isDashing = false;
        Time.timeScale = 1f;
    }

    private IEnumerator CO_TapLeap()
    {
        Debug.Log("TapLeap was called");
        _isTapLeaping = true;

        Vector3 topPosition = _startPosition + Vector3.up * _leapHeight;

        while (Vector3.Distance(transform.position, topPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                topPosition,
                _leapSpeed * Time.deltaTime);

            yield return null;
        }

        while (Vector3.Distance(transform.position, _startPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _startPosition,
                _leapSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = _startPosition;
        _isTapLeaping = false;
    }
}
