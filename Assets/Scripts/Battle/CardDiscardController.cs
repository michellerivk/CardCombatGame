using System.Collections.Generic;
using UnityEngine;

public class CardDiscardController : MonoBehaviour
{
    [SerializeField] private CardPlacePoint[] _placements;
    [SerializeField] private Transform _discardPoint;

    private readonly HashSet<Card> _cardsBeingDiscarded = new HashSet<Card>();

    private void OnEnable()
    {
        if (_placements == null) return;

        foreach (CardPlacePoint point in _placements)
        {
            if (point == null) continue;
            point.OnCardDefeated -= DiscardDefeatedCard;
            point.OnCardDefeated += DiscardDefeatedCard;
        }
    }

    private void OnDisable()
    {
        if (_placements == null) return;

        foreach (CardPlacePoint point in _placements)
        {
            if (point != null)
                point.OnCardDefeated -= DiscardDefeatedCard;
        }
    }

    private void DiscardDefeatedCard(Card card)
    {
        DiscardCard(card);
    }

    public void DiscardAllCards(IReadOnlyList<Card> cardsFromHand)
    {
        HashSet<Card> cardsToDiscard = new HashSet<Card>();

        if (cardsFromHand != null)
        {
            foreach (Card card in cardsFromHand)
            {
                if (card != null)
                    cardsToDiscard.Add(card);
            }
        }

        if (_placements != null)
        {
            foreach (CardPlacePoint point in _placements)
            {
                if (point == null || point.ActiveCard == null)
                    continue;

                Card card = point.ActiveCard;
                point.Release(card);
                cardsToDiscard.Add(card);
            }
        }

        foreach (Card card in cardsToDiscard)
            DiscardCard(card);
    }

    private void DiscardCard(Card card)
    {
        if (card == null || !_cardsBeingDiscarded.Add(card))
            return;

        // The square has already released the card. Stop interaction during travel.
        if (card.TryGetComponent(out CardSelectionController selection))
            selection.UnSelectCard();
        if (card.TryGetComponent(out CardPointerInput input))
            input.enabled = false;
        if (card.TryGetComponent(out CardPlacementController placement))
            placement.enabled = false;
        foreach (Collider cardCollider in card.GetComponentsInChildren<Collider>())
            cardCollider.enabled = false;

        if (_discardPoint == null || !card.TryGetComponent(out CardMotion motion) ||
            !motion.isActiveAndEnabled)
        {
            Debug.LogError("Discarding a card needs a Discard Point and an enabled CardMotion.", this);
            Destroy(card.gameObject);
            return;
        }

        if (card.TryGetComponent(out CardAnimator cardAnimator) && cardAnimator.isActiveAndEnabled)
            cardAnimator.AnimateJump();

        motion.MoveTo(_discardPoint.position, _discardPoint.rotation, () =>
        {
            if (card != null)
                Destroy(card.gameObject);
        });
    }
}
