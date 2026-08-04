using TMPro;
using UnityEngine;
using Castillo.Loot;

namespace Castillo.UI
{
    public class UI_HealthPickup : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HealthInventory _inventory;
        [SerializeField] private HealthPickupUse _healthPickupUse;

        [Header("UI")]
        [SerializeField] private GameObject _background;
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private TMP_Text _countdownText;

        private void OnEnable()
        {
            _inventory.AmountChanged += OnAmountChanged;
            _healthPickupUse.UseProgressChanged += OnUseProgressChanged;
            _healthPickupUse.UseStateChanged += OnUseStateChanged;
        }

        private void OnDisable()
        {
            _inventory.AmountChanged -= OnAmountChanged;
            _healthPickupUse.UseProgressChanged -= OnUseProgressChanged;
            _healthPickupUse.UseStateChanged -= OnUseStateChanged;
        }

        private void Start()
        {
            RefreshInventoryDisplay();

            _countdownText.gameObject.SetActive(false);
        }

        private void OnAmountChanged(
            int currentAmount,
            int maximumAmount
        )
        {
            RefreshInventoryDisplay();
        }

        private void RefreshInventoryDisplay()
        {
            bool hasHealthPickup =
                _inventory.CurrentAmount > 0;

            _background.SetActive(hasHealthPickup);
            _amountText.gameObject.SetActive(hasHealthPickup);

            _amountText.text =
                _inventory.CurrentAmount.ToString();
        }

        private void OnUseStateChanged(bool isUsing)
        {
            _countdownText.gameObject.SetActive(isUsing);

            if (!isUsing)
            {
                _countdownText.text = string.Empty;
            }
        }

        private void OnUseProgressChanged(
            float elapsedTime,
            float duration
        )
        {
            float remainingTime =
                Mathf.Max(duration - elapsedTime, 0f);

            _countdownText.text =
                remainingTime.ToString("00.00");
        }
    }
}