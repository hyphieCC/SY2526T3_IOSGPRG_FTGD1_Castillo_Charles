using UnityEngine;
using UnityEngine.InputSystem;
using Castillo.Combat;

namespace SurvivIo.Debugging
{
    public class HealthDebugTest : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private float _testDamage = 10f;

        private void Update()
        {
            if (Keyboard.current == null)
            {
                return;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                _health.TakeDamage(_testDamage);

                Debug.Log($"Player health: {_health.CurrentHealth}/{_health.MaximumHealth}");
            }
        }
    }
}