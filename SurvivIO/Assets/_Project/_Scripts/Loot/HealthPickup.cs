using UnityEngine;

namespace Castillo.Loot
{
    public class HealthPickup : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            HealthInventory inventory = other.GetComponentInParent<HealthInventory>();

            if (inventory == null)
            {
                return;
            }

            if (!inventory.TryAddHealthPickup())
            {
                return;
            }

            Destroy(gameObject);
        }
    }
}