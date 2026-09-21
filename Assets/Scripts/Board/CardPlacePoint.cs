using UnityEngine;

public class CardPlacePoint : MonoBehaviour
{
    [SerializeField] private Card _activeCard;
    [SerializeField] private bool _isPlayerPoint;

    public bool TryAssign(Card card)
    {
        if (card == null || _activeCard != null || !_isPlayerPoint)
            return false;

        _activeCard = card;
        return true;
    }

    public void Release(Card card)
    {
        if (_activeCard == card)
            _activeCard = null;
    }
}
