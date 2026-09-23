using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private CardDeck _deck;
    [SerializeField] private Transform _cardSpawnPoint;
    [SerializeField] private Card _cardToSpawn;
    [SerializeField] private CardPlacePoint[] _placements;
    [SerializeField, Min(0)] private int _cardsPerTurn = 1;
    [SerializeField, Min(0f)] private float _timeBetweenPlays = 0.5f;

    // BattleController waits for this routine before starting enemy attacks.
    public IEnumerator RunTurn()
    {
        if (_deck == null || _cardSpawnPoint == null || _cardToSpawn == null || _placements == null)
        {
            Debug.LogError("EnemyController needs a deck, spawn point, card prefab, and board slots.", this);
            yield break;
        }

        if (!_cardToSpawn.TryGetComponent(out CardMotion prefabMotion) || !prefabMotion.enabled)
        {
            Debug.LogError("The enemy card prefab needs an enabled CardMotion.", this);
            yield break;
        }

        for (int i = 0; i < _cardsPerTurn; i++)
        {
            yield return new WaitForSeconds(_timeBetweenPlays);

            CardPlacePoint point = ChooseEmptyPoint();
            // Don't consume a card when there is nowhere to play it.
            if (point == null || !_deck.TryDraw(out CardSO definition))
                yield break;

            Card card = Instantiate(_cardToSpawn, _cardSpawnPoint.position, _cardToSpawn.transform.rotation);
            card.Initialize(definition);
            DisablePlayerInteraction(card);

            if (!point.TryAssign(card))
            {
                Destroy(card.gameObject);
                yield break;
            }

            CardMotion motion = card.GetComponent<CardMotion>();
            bool arrived = false;
            // Keep the enemy prefab's facing direction.
            motion.MoveTo(point.transform.position, card.transform.rotation, () => arrived = true);

            // A defeated card may get a new discard movement before it arrives.
            while (!arrived && card != null && !card.IsDefeated &&
                   motion != null && motion.isActiveAndEnabled)
            {
                yield return null;
            }
        }
    }

    private CardPlacePoint ChooseEmptyPoint()
    {
        var emptyPoints = new List<CardPlacePoint>();
        foreach (CardPlacePoint point in _placements)
        {
            if (point != null && !point.IsPlayerPoint && point.ActiveCard == null)
                emptyPoints.Add(point);
        }

        return emptyPoints.Count == 0 ? null : emptyPoints[Random.Range(0, emptyPoints.Count)];
    }

    private static void DisablePlayerInteraction(Card card)
    {
        if (card.TryGetComponent(out CardSelectionController selection))
        {
            selection.UnSelectCard();
            selection.enabled = false;
        }
        if (card.TryGetComponent(out CardPointerInput input))
            input.enabled = false;
        if (card.TryGetComponent(out CardPlacementController placement))
            placement.enabled = false;
        if (card.TryGetComponent(out CardHandState handState))
            handState.RemoveFromHand();
    }
}
