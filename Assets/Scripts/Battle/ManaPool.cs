using System;
using UnityEngine;

public class ManaPool : MonoBehaviour
{
    [SerializeField] private int _startingMana = 4;
    [SerializeField] private int _maximumMana = 12;

    private int _currentMaxMana;

    public int Current { get; private set; }
    public int Maximum => _maximumMana;

    public event Action<int, int> OnManaChanged;
    public event Action OnNotEnoughMana;

    private void Awake()
    {
        Initialize(_startingMana, _maximumMana);
    }

    public void Initialize(int startingMana, int maximumMana)
    {
        _maximumMana = Mathf.Max(0, maximumMana);
        _startingMana = Mathf.Clamp(startingMana, 0, _maximumMana);
        _currentMaxMana = _startingMana;
        Current = _currentMaxMana;
        OnManaChanged?.Invoke(Current, Maximum);
    }
    public bool CanAfford(int cost)
    {
        return cost >= 0 && Current >= cost;
    }

    // Use this when an attempted action should also show player feedback.
    public bool CheckCanAfford(int cost)
    {
        if (CanAfford(cost))
            return true;

        OnNotEnoughMana?.Invoke();
        return false;
    }

    public bool TrySpend(int cost)
    {
        if (!CheckCanAfford(cost))
            return false;

        Current -= cost;
        OnManaChanged?.Invoke(Current, Maximum);
        return true;
    }
    public void UpdateMana()
    {
        if(_currentMaxMana < Maximum)
            _currentMaxMana++;

        FillMana();
    }
    private void FillMana()
    {
        Current = _currentMaxMana;
        OnManaChanged?.Invoke(Current, Maximum);
    }
}
