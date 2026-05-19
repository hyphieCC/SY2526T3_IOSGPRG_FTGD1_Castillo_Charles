using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    List<Enemy> _enemies = new List<Enemy>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            _enemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            _enemies.Remove(enemy);
        }
    }

    public void CheckSwipe(SwipeDirection direction)
    {
        if (_enemies.Count <= 0)
        {
            return;
        }

        Enemy enemy = _enemies[0];

        if (enemy.CheckPlayerSwipe(direction))
        {
            _enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }
    }
}
