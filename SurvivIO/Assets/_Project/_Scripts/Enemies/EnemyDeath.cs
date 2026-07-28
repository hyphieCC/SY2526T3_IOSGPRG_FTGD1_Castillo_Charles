using UnityEngine;
using Castillo.Combat;

namespace Castillo.Enemies
{
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private Health _health;

        private void OnEnable()
        {
            _health.Died += Die;
        }

        private void OnDisable()
        {
            _health.Died -= Die;
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}