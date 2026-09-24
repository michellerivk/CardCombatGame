using System.Collections;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    [SerializeField] private CardDeck _deck;
    [SerializeField] private Card _cardToSpawn;
    [SerializeField] private PlayerBattleContext _playerContext;
    [SerializeField, Min(0f)] private float waitBetweenDrawingCards = 0.25f;

    private bool _acceptDrawRequests = true;

    public bool CanDraw => _acceptDrawRequests && _deck != null && _deck.CanDraw;

    private bool TryDrawCardToHand()
    {
        if (_deck == null || _cardToSpawn == null || _playerContext == null)
        {
            Debug.LogError("Drawing to hand needs a CardDeck, card prefab, and PlayerBattleContext.", this);
            return false;
        }

        if (!_deck.TryDraw(out CardSO definition))
            return false;

        Card newCard = Instantiate(_cardToSpawn, transform.position, transform.rotation);
        newCard.Initialize(definition);
        _playerContext.AddCardToHand(newCard);
        return true;
    }

    public void DrawCardsToHand(int count)
    {
        if (!_acceptDrawRequests || count <= 0)
            return;

        StartCoroutine(DrawCardsToHandCo(count));
    }

    // DeckController owns its draw coroutines, so it also owns cancelling them.
    public void CancelPendingDraws()
    {
        _acceptDrawRequests = false;
        StopAllCoroutines();
    }

    private IEnumerator DrawCardsToHandCo(int amountToDraw)
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            if (!TryDrawCardToHand())
                yield break;

            yield return new WaitForSeconds(waitBetweenDrawingCards);
        }
    }
}
