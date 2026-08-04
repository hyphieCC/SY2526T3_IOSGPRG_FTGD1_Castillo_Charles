using System;
using UnityEngine;

namespace Castillo.Loot
{
    public class HealthInventory : MonoBehaviour
    {
        [SerializeField] private int _maximumCarry = 3;

        public int CurrentAmount { get; private set; }
        public int MaximumCarry => _maximumCarry;

        public event Action<int, int> AmountChanged;

        public bool TryAddHealthPickup()
        {
            if (CurrentAmount >= _maximumCarry)
            {
                return false;
            }

            CurrentAmount++;

            AmountChanged?.Invoke(CurrentAmount, _maximumCarry);

            return true;
        }

        public bool TryConsumeHealthPickup()
        {
            if (CurrentAmount <= 0)
            {
                return false;
            }

            CurrentAmount--;

            AmountChanged?.Invoke(CurrentAmount, _maximumCarry);

            return true;
        }
    }
}