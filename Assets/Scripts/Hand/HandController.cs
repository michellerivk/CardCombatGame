using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private List<Card> _heldCards = new List<Card>();
    [SerializeField] private Transform _minPos, _maxPos;

    public IReadOnlyList<Card> HeldCards => _heldCards;

    private void Start()
    {
        SetCardPositionsInHand();
    }

    private void SetCardPositionsInHand()
    {
        Vector3 distanceBetweenPoints = Vector3.zero;
        if (_heldCards.Count > 1)
            distanceBetweenPoints = (_maxPos.position - _minPos.position) / (_heldCards.Count - 1);

        for (int i = 0; i < _heldCards.Count; i++)
        {
            Card card = _heldCards[i];
            if (!card.TryGetComponent(out CardHandState handState) ||
                !card.TryGetComponent(out CardMotion motion))
            {
                Debug.LogError($"{card.name} needs CardHandState and CardMotion components.", card);
                continue;
            }

            handState.PlaceAt(i);
            motion.SetHandPose(_minPos.position + distanceBetweenPoints * i, _minPos.rotation);
        }
    }

    public bool RemoveCardFromHand(CardHandState cardToRemove)
    {
        if (cardToRemove == null || !cardToRemove.TryGetComponent(out Card card))
        {
            Debug.LogError("The card being removed needs Card and CardHandState components.");
            return false;
        }

        int cardIndex = _heldCards.IndexOf(card);
        if (cardIndex < 0)
        {
            Debug.LogError($"{card.name} is not registered in this hand.", card);
            return false;
        }

        _heldCards.RemoveAt(cardIndex);
        cardToRemove.RemoveFromHand();
        SetCardPositionsInHand();
        return true;
    }

    public void AddCard(Card card)
    {
        if (card == null || _heldCards.Contains(card))
            return;

        _heldCards.Add(card);
        SetCardPositionsInHand();
    }

    // Removes ownership of every card and returns the cards to the caller that
    // decides where they should visually go (for example, the discard pile).
    public List<Card> EmptyHand()
    {
        List<Card> removedCards = new List<Card>(_heldCards);

        foreach (Card heldCard in removedCards)
        {
            if (heldCard == null)
                continue;

            if (!heldCard.TryGetComponent(out CardHandState cardHandState))
            {
                Debug.LogError($"{heldCard.name} needs a CardHandState component.", heldCard);
                continue;
            }

            cardHandState.RemoveFromHand();
        }

        _heldCards.Clear();
        return removedCards;
    }
}
