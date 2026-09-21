using System;
using UnityEngine;

public class CardSelectionController : MonoBehaviour
{
    [SerializeField] private CardPointerInput _input;

    [SerializeField] private bool _isSelected;
    [SerializeField] private Collider _collider;

    public bool IsSelected => _isSelected;

    public event Action<bool> OnSelectionChanged;

    private void Awake()
    {
        if (_collider == null) _collider = GetComponent<Collider>();
        if (_input == null) _input = GetComponent<CardPointerInput>();
    }

    private void OnEnable()
    {
        _input.OnSelect += SelectCard;
        _input.OnCancelSelection += UnSelectCard;
    }

    private void OnDisable()
    {
        _input.OnSelect -= SelectCard;
        _input.OnCancelSelection -= UnSelectCard;
    }

    private void SelectCard()
    {
        _isSelected = true;
        _collider.enabled = false;

        OnSelectionChanged?.Invoke(_isSelected);
    }

    public void UnSelectCard()
    {
        if (!_isSelected)
            return;

        _isSelected = false;
        _collider.enabled = true;

        OnSelectionChanged?.Invoke(_isSelected);
    }
}
