using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCleanUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            Destroy(enemy.gameObject);
        }
    }
}
