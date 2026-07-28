using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Castillo.Loot
{
    public class AmmoInventory : MonoBehaviour
    {
        [Header("Maximum Carry")]
        [SerializeField] private int _maximumNineMillimeter = 90;
        [SerializeField] private int _maximumTwelveGauge = 60;
        [SerializeField] private int _maximumFiveFiveSixMillimeter = 120;

        [Header("Starting Ammo")]
        [SerializeField] private int _startingNineMillimeter;
        [SerializeField] private int _startingTwelveGauge;
        [SerializeField] private int _startingFiveFiveSixMillimeter;

        public int NineMillimeter { get; private set; }
        public int TwelveGauge { get; private set; }
        public int FiveFiveSixMillimeter { get; private set; }

        public event Action<AmmoType, int> AmmoChanged;

        private void Awake()
        {
            NineMillimeter = Mathf.Clamp(
                _startingNineMillimeter,
                0,
                _maximumNineMillimeter
            );

            TwelveGauge = Mathf.Clamp(
                _startingTwelveGauge,
                0,
                _maximumTwelveGauge
            );

            FiveFiveSixMillimeter = Mathf.Clamp(
                _startingFiveFiveSixMillimeter,
                0,
                _maximumFiveFiveSixMillimeter
            );
        }

        public int AddAmmo(AmmoType ammoType, int amount)
        {
            if (amount <= 0)
            {
                return 0;
            }

            int amountAdded = 0;

            switch (ammoType)
            {
                case AmmoType.NineMillimeter:
                    {
                        amountAdded = AddNineMillimeter(amount);
                        break;
                    }

                case AmmoType.TwelveGauge:
                    {
                        amountAdded = AddTwelveGauge(amount);
                        break;
                    }

                case AmmoType.FiveFiveSixMillimeter:
                    {
                        amountAdded = AddFiveFiveSixMillimeter(amount);
                        break;
                    }
            }

            if (amountAdded > 0)
            {
                AmmoChanged?.Invoke(ammoType, GetAmmoAmount(ammoType));
            }

            return amountAdded;
        }

        public int GetAmmoAmount(AmmoType ammoType)
        {
            switch (ammoType)
            {
                case AmmoType.NineMillimeter:
                    {
                        return NineMillimeter;
                    }

                case AmmoType.TwelveGauge:
                    {
                        return TwelveGauge;
                    }

                case AmmoType.FiveFiveSixMillimeter:
                    {
                        return FiveFiveSixMillimeter;
                    }

                default:
                    {
                        return 0;
                    }
            }
        }

        public int GetMaximumAmmo(AmmoType ammoType)
        {
            switch (ammoType)
            {
                case AmmoType.NineMillimeter:
                    {
                        return _maximumNineMillimeter;
                    }

                case AmmoType.TwelveGauge:
                    {
                        return _maximumTwelveGauge;
                    }

                case AmmoType.FiveFiveSixMillimeter:
                    {
                        return _maximumFiveFiveSixMillimeter;
                    }

                default:
                    {
                        return 0;
                    }
            }
        }

        public int RemoveAmmo(AmmoType ammoType, int requestedAmount)
        {
            if (requestedAmount <= 0)
            {
                return 0;
            }

            int amountRemoved = 0;

            switch (ammoType)
            {
                case AmmoType.NineMillimeter:
                    {
                        amountRemoved = RemoveNineMillimeter(requestedAmount);
                        break;
                    }

                case AmmoType.TwelveGauge:
                    {
                        amountRemoved = RemoveTwelveGauge(requestedAmount);
                        break;
                    }

                case AmmoType.FiveFiveSixMillimeter:
                    {
                        amountRemoved = RemoveFiveFiveSixMillimeter(requestedAmount);
                        break;
                    }

                default:
                    {
                        return 0;
                    }
            }

            if (amountRemoved > 0)
            {
                AmmoChanged?.Invoke(ammoType, GetAmmoAmount(ammoType));
            }

            return amountRemoved;
        }

        private int AddNineMillimeter(int amount)
        {
            int previousAmount = NineMillimeter;

            NineMillimeter = Mathf.Min(NineMillimeter + amount, _maximumNineMillimeter);

            return NineMillimeter - previousAmount;
        }

        private int AddTwelveGauge(int amount)
        {
            int previousAmount = TwelveGauge;

            TwelveGauge = Mathf.Min(TwelveGauge + amount, _maximumTwelveGauge);

            return TwelveGauge - previousAmount;
        }

        private int AddFiveFiveSixMillimeter(int amount)
        {
            int previousAmount = FiveFiveSixMillimeter;

            FiveFiveSixMillimeter = Mathf.Min(FiveFiveSixMillimeter + amount, _maximumFiveFiveSixMillimeter);

            return FiveFiveSixMillimeter - previousAmount;
        }

        private int RemoveNineMillimeter(int requestedAmount)
        {
            int amountRemoved = Mathf.Min(
                requestedAmount,
                NineMillimeter
            );

            NineMillimeter -= amountRemoved;

            return amountRemoved;
        }

        private int RemoveTwelveGauge(int requestedAmount)
        {
            int amountRemoved = Mathf.Min(
                requestedAmount,
                TwelveGauge
            );

            TwelveGauge -= amountRemoved;

            return amountRemoved;
        }

        private int RemoveFiveFiveSixMillimeter(int requestedAmount)
        {
            int amountRemoved = Mathf.Min(
                requestedAmount,
                FiveFiveSixMillimeter
            );

            FiveFiveSixMillimeter -= amountRemoved;

            return amountRemoved;
        }
    }
}
