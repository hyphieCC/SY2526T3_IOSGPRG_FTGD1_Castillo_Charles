using TMPro;
using UnityEngine;
using Castillo.Loot;

namespace Castillo.UI
{
    public class UI_AmmoInventory : MonoBehaviour
    {
        [SerializeField] private AmmoInventory _ammoInventory;

        [Header("Ammo Text")]
        [SerializeField] private TMP_Text _nineMillimeterText;
        [SerializeField] private TMP_Text _twelveGaugeText;
        [SerializeField] private TMP_Text _fiveFiveSixText;

        private void Start()
        {
            UpdateAllAmmoText();
        }

        private void OnEnable()
        {
            _ammoInventory.AmmoChanged += UpdateAmmoText;
        }

        private void OnDisable()
        {
            _ammoInventory.AmmoChanged -= UpdateAmmoText;
        }

        private void UpdateAmmoText(AmmoType ammoType, int currentAmount)
        {
            switch (ammoType)
            {
                case AmmoType.NineMillimeter:
                    {
                        UpdateNineMillimeterText();
                        break;
                    }

                case AmmoType.TwelveGauge:
                    {
                        UpdateTwelveGaugeText();
                        break;
                    }

                case AmmoType.FiveFiveSixMillimeter:
                    {
                        UpdateFiveFiveSixText();
                        break;
                    }
            }
        }

        private void UpdateAllAmmoText()
        {
            UpdateNineMillimeterText();
            UpdateTwelveGaugeText();
            UpdateFiveFiveSixText();
        }

        private void UpdateNineMillimeterText()
        {
            _nineMillimeterText.text = _ammoInventory.GetAmmoAmount(AmmoType.NineMillimeter).ToString();
        }

        private void UpdateTwelveGaugeText()
        {
            _twelveGaugeText.text = _ammoInventory.GetAmmoAmount(AmmoType.TwelveGauge).ToString();
        }

        private void UpdateFiveFiveSixText()
        {
            _fiveFiveSixText.text = _ammoInventory.GetAmmoAmount(AmmoType.FiveFiveSixMillimeter).ToString();
        }
    }
}