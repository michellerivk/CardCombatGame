using UnityEngine;

public class CardPlacePoint : MonoBehaviour
{
    [SerializeField] private Card _activeCard;
    [SerializeField] private bool _isPlayerPoint;

    public Card ActiveCard => _activeCard;
    public bool IsPlayerPoint => _isPlayerPoint;

    public bool TryAssign(Card card)
    {
        if (card == null || _activeCard != null)
            return false;

        _activeCard = card;
        card.OnDefeated += Release;

        return true;
    }

    public void Release(Card card)
    {
        if (_activeCard == card)
        {
            card.OnDefeated -= Release;
            _activeCard = null;
        }
    }
}
