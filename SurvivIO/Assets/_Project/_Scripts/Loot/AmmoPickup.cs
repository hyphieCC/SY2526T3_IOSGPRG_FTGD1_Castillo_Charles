using UnityEngine;

namespace Castillo.Loot
{
    public class AmmoPickup : MonoBehaviour
    {
        [SerializeField] private AmmoType _ammoType;
        [SerializeField] private int _minimumAmount = 1;
        [SerializeField] private int _maximumAmount = 1;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out AmmoInventory ammoInventory))
            {
                return;
            }

            int ammoAmount = Random.Range(_minimumAmount, _maximumAmount + 1);

            int amountAdded = ammoInventory.AddAmmo(_ammoType, ammoAmount);

            if (amountAdded <= 0)
            {
                return;
            }

            Destroy(gameObject);
        }
    }
}