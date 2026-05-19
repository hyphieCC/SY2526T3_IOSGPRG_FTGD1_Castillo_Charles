using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Timer : MonoBehaviour
{
    [SerializeField] private bool _detectedByPlayer;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Sprite> _arrowSprites = new List<Sprite>();

    private void Start()
    {
        StartCoroutine(CO_SpawnEnemyEveryXSeconds(5));
    }

    private IEnumerator CO_ArrowRotation()
    {
        int index = 0;

        while (!_detectedByPlayer)
        {
            _spriteRenderer.sprite = _arrowSprites[index % 4];
            index++;
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    private IEnumerator CO_SpawnEnemyEveryXSeconds(float seconds)
    {
        float currentTime = 0f;

        while (true)
        {
            currentTime += Time.deltaTime;
            //Debug.Log($"Spawn Timer: {currentTime}");
            if (currentTime >= seconds)
            {
                //Spawner.Instance.SpawnEnemy();
                currentTime = 0f;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CO_CountDownTimer(float startTime)
    {
        float currentTime = startTime;
        
        while (currentTime > 0)
        {
            Debug.Log($"Current Time: {currentTime}");
            yield return new WaitForSecondsRealtime(1f);
            currentTime--;
        }

        Debug.Log("Delayed Function Goes Here");
    }

    private IEnumerator CO_CountUpTimer()
    {
        float currentTime = 0;

        while (true)
        {
            Debug.Log($"Current Time: {currentTime}");
            yield return new WaitForSecondsRealtime(1f);
            currentTime++;
        }
    }
}
