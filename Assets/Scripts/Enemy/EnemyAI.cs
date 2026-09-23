using System;
using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public enum AIType{ placeFromDeck, handRandomPlace, handDefensive, handAttacking }
    [SerializeField] private AIType _enemyAIType;

    public AIType CurrentType => _enemyAIType;

    public event Action<AIType> OnAIChanged;

    private readonly IEnemyPlacementStrategy _randomPlacement = new RandomEnemyPlacementStrategy();
    private readonly IEnemyPlacementStrategy _defensivePlacement = new DefensiveEnemyPlacementStrategy();
    private readonly IEnemyPlacementStrategy _attackingPlacement = new AttackingEnemyPlacementStrategy();

    // Choose without spending mana or removing cards from the supplied collection.
    // Hand mode supplies its hand; deck mode supplies its pending drawn card.
    public CardSO ChoosePlayableCard(IReadOnlyList<CardSO> candidates, int availableMana)
    {
        if (candidates == null || candidates.Count == 0 || availableMana < 0)
            return null;

        switch (_enemyAIType)
        {
            case AIType.placeFromDeck:

            case AIType.handRandomPlace:

            case AIType.handDefensive:

            case AIType.handAttacking:
                var playableCards = new List<CardSO>();
                foreach (CardSO card in candidates)
                {
                    if (card != null && card.currentHealth > 0 &&
                        card.manaCost >= 0 && card.manaCost <= availableMana)
                    {
                        playableCards.Add(card);
                    }
                }

                return playableCards.Count == 0
                    ? null
                    : playableCards[UnityEngine.Random.Range(0, playableCards.Count)];

            // These strategies will get their own selection rules later.
            default:
                return null;
        }
    }

    public CardPlacePoint ChoosePlacement(BoardLayout board)
    {
        if (board == null || !board.ValidateSetup())
            return null;

        IEnemyPlacementStrategy strategy;
        switch (_enemyAIType)
        {
            case AIType.placeFromDeck:

            case AIType.handRandomPlace:
                strategy = _randomPlacement;
                break;

            case AIType.handDefensive:
                strategy = _defensivePlacement;
                break;

            case AIType.handAttacking:
                strategy = _attackingPlacement;
                break;

            default:
                return null;
        }

        return strategy.ChoosePlacement(board.EnemyPoints, board.PlayerPoints);
    }
}
