using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMode : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_player.IsDashing)
        {
            return;
        }

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            _player.KillEnemy(enemy);
        }
    }
}
