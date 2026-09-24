using System;
using UnityEngine;

public class CardPlacePoint : MonoBehaviour
{
    [SerializeField] private Card _activeCard;
    [SerializeField] private bool _isPlayerPoint;

    public Card ActiveCard => _activeCard;
    public bool IsPlayerPoint => _isPlayerPoint;
    public event Action<Card> OnCardAssigned;
    public event Action<Card> OnCardDefeated;

    private void Awake()
    {
        // Scene-assigned occupants need the same subscription as newly placed cards.
        if (_activeCard != null)
        {
            _activeCard.OnDefeated -= HandleCardDefeated;
            _activeCard.OnDefeated += HandleCardDefeated;
        }
    }

    private void OnDestroy()
    {
        if (_activeCard != null)
            _activeCard.OnDefeated -= HandleCardDefeated;
    }

    public bool TryAssign(Card card)
    {
        if (card == null || card.IsDefeated || _activeCard != null)
            return false;

        _activeCard = card;
        card.OnDefeated += HandleCardDefeated;
        OnCardAssigned?.Invoke(card);

        return true;
    }

    public void Release(Card card)
    {
        if (card != null && _activeCard == card)
        {
            card.OnDefeated -= HandleCardDefeated;
            _activeCard = null;
        }
    }

    private void HandleCardDefeated(Card card)
    {
        Release(card);
        OnCardDefeated?.Invoke(card);
    }
}
