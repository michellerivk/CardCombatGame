using System;
using UnityEngine;

public class ManaPool : MonoBehaviour
{
    [SerializeField] private int _startingMana = 4;
    [SerializeField] private int _maximumMana = 12;

    private int _playerCurrentMaxMana;

    public int Current { get; private set; }
    public int Maximum => _maximumMana;

    public event Action<int, int> OnManaChanged;
    public event Action OnNotEnoughMana;

    private void Awake()
    {
        _playerCurrentMaxMana = _startingMana;
        Current = _playerCurrentMaxMana;
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
    public void UpdatePlayerMana()
    {
        if(_playerCurrentMaxMana < Maximum)
            _playerCurrentMaxMana++;

        FillPlayerMana();
    }
    private void FillPlayerMana()
    {
        Current = _playerCurrentMaxMana;
        OnManaChanged?.Invoke(Current, Maximum);
    }
}
