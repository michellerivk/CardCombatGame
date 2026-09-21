using System;
using UnityEngine;

public class ManaPool : MonoBehaviour
{
    [SerializeField] private int _startingMana = 4;
    [SerializeField] private int _maximumMana = 12;

    public int Current { get; private set; }
    public int Maximum => _maximumMana;

    public event Action<int, int> OnManaChanged;
    public event Action OnNotEnoughMana;

    private void Awake()
    {
        Current = Mathf.Clamp(_startingMana, 0, _maximumMana);
    }
    public bool CanAfford(int cost)
    {
        return cost >= 0 && Current >= cost;
    }

    public bool TrySpend(int cost)
    {
        if (!CanAfford(cost))
        {
            OnNotEnoughMana?.Invoke();
            return false;
        }

        Current -= cost;
        OnManaChanged?.Invoke(Current, Maximum);
        return true;
    }
}
