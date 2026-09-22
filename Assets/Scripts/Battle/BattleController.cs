using System;
using System.Collections;
using UnityEngine;

public enum TurnOrder {playerActive, playerCardAttacks, enemyActive, enemyCardAttacks}

public class BattleController : MonoBehaviour
{
    [Header("Mana")]
    [SerializeField] private ManaPool _playerMana;
    [SerializeField] private ManaPool _enemyMana;

    [Header("Starting Deck")]
    [SerializeField] private DeckController _playerDeck;
    [SerializeField] private int _openingHandSize = 5;

    [Header("Turns")]
    [SerializeField] private TurnOrder _currentPhase;
    [SerializeField] private int _cardsToDrawPerTurn = 1;

    [Header("References")]
    [SerializeField] private CardsPointController _cardsPointController;

    public TurnOrder CurrentPhase => _currentPhase;
    public event Action<TurnOrder> OnPhaseChanged; // CardPlacePoint listens


    private void Start()
    {
        _playerDeck.DrawCardsToHand(_openingHandSize);
    }


    private void AdvanceTurn()
    {
        _currentPhase++;
        
        if((int)_currentPhase >= Enum.GetValues(typeof(TurnOrder)).Length)
            _currentPhase = 0;

        OnPhaseChanged?.Invoke(_currentPhase);

        switch (_currentPhase)
        {
            case TurnOrder.playerActive:
                _playerMana.UpdatePlayerMana();
                _playerDeck.DrawCardsToHand(_cardsToDrawPerTurn);
                break;

            case TurnOrder.playerCardAttacks:
                StartCoroutine(RunPlayerAttackPhase());
                break;

            case TurnOrder.enemyActive:
                StartCoroutine(RunEnemyAttacksPhase());
                break;

            case TurnOrder.enemyCardAttacks:
                AdvanceTurn();
                break;
        }

    }

    public void EndPlayerTurn()
    {
        if (_currentPhase != TurnOrder.playerActive)
            return;

        AdvanceTurn();
    }

    private IEnumerator RunPlayerAttackPhase()
    {
        yield return StartCoroutine(_cardsPointController.RunPlayerAttacks());
        AdvanceTurn();
    }
    private IEnumerator RunEnemyAttacksPhase()
    {
        yield return StartCoroutine(_cardsPointController.RunEnemyAttacks());
        AdvanceTurn();
    }
}
