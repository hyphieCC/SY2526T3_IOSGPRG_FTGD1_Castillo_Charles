using UnityEngine;
using UnityEngine.InputSystem;
using Castillo.Loot;

namespace SurvivIo.Debugging
{
    public class AmmoInventoryDebugTest : MonoBehaviour
    {
        [SerializeField] private AmmoInventory _ammoInventory;

        private void Update()
        {
            if (Keyboard.current == null)
            {
                return;
            }

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                AddTestAmmo(AmmoType.NineMillimeter, 8);
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                AddTestAmmo(AmmoType.TwelveGauge, 2);
            }

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                AddTestAmmo(AmmoType.FiveFiveSixMillimeter, 15);
            }
        }

        private void AddTestAmmo(AmmoType ammoType, int amount)
        {
            int amountAdded = _ammoInventory.AddAmmo(ammoType, amount);

            Debug.Log(
                $"Added {amountAdded} {ammoType}. " +
                $"Current: {_ammoInventory.GetAmmoAmount(ammoType)}/" +
                $"{_ammoInventory.GetMaximumAmmo(ammoType)}"
            );
        }
    }
}