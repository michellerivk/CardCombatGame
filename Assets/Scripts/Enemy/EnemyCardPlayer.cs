using System;
using UnityEngine;

// Executes a requested play; does not choose cards or own a hand/deck.
public class EnemyCardPlayer : MonoBehaviour
{
    [SerializeField] private Transform _cardSpawnPoint;
    [SerializeField] private Card _cardToSpawn;

    public bool ValidateSetup()
    {
        if (_cardSpawnPoint == null || _cardToSpawn == null)
        {
            Debug.LogError("EnemyCardPlayer needs a spawn point and card prefab.", this);
            return false;
        }

        if (!_cardToSpawn.TryGetComponent(out CardMotion motion) || !motion.enabled)
        {
            Debug.LogError("The enemy card prefab needs an enabled CardMotion.", this);
            return false;
        }

        return true;
    }

    // The caller removes the source card only when this returns true.
    public bool TryPlay(CardSO definition, CardPlacePoint point, ManaPool mana,
        out Card playedCard, Action onArrived = null)
    {
        playedCard = null;
        if (definition == null || definition.currentHealth <= 0 || point == null ||
            point.IsPlayerPoint || point.ActiveCard != null || mana == null ||
            !mana.CanAfford(definition.manaCost))
        {
            return false;
        }

        if (!ValidateSetup())
            return false;

        Card newCard = Instantiate(_cardToSpawn, _cardSpawnPoint.position, _cardToSpawn.transform.rotation);
        newCard.Initialize(definition);
        DisablePlayerInteraction(newCard);

        if (!newCard.TryGetComponent(out CardMotion motion) || !motion.isActiveAndEnabled)
        {
            Debug.LogError("Playing an enemy card needs an enabled CardMotion.", this);
            Destroy(newCard.gameObject);
            return false;
        }

        if (!point.TryAssign(newCard))
        {
            Destroy(newCard.gameObject);
            return false;
        }

        if (!mana.TrySpend(newCard.ManaCost))
        {
            point.Release(newCard);
            Destroy(newCard.gameObject);
            return false;
        }

        // Keep the enemy prefab's facing direction.
        motion.MoveTo(point.transform.position, newCard.transform.rotation, onArrived);
        playedCard = newCard;
        return true;
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
