using UnityEngine;

public class CardDiscardController : MonoBehaviour
{
    [SerializeField] private CardPlacePoint[] _placements;
    [SerializeField] private Transform _discardPoint;

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
        if (card == null) return;

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
