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
    // Shared by the player and enemy at the start of their respective turns.
    [SerializeField, Min(0)] private int _cardsToDrawPerTurn = 1;

    [Header("References")]
    [SerializeField] private CardsPointController _cardsPointController;
    [SerializeField] private EnemyController _enemyController;

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
                _playerMana.UpdateMana();
                _playerDeck.DrawCardsToHand(_cardsToDrawPerTurn);
                break;

            case TurnOrder.playerCardAttacks:
                StartCoroutine(RunPlayerAttackPhase());
                break;

            case TurnOrder.enemyActive:
                _enemyMana.UpdateMana();
                StartCoroutine(RunEnemyActionPhase());
                break;

            case TurnOrder.enemyCardAttacks:
                StartCoroutine(RunEnemyAttacksPhase());
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
    private IEnumerator RunEnemyActionPhase()
    {
        if (_enemyController == null)
        {
            Debug.LogError("BattleController needs an EnemyController.", this);
            yield break;
        }

        _enemyController.DrawCardsToHand(_cardsToDrawPerTurn);
        yield return StartCoroutine(_enemyController.RunTurn());
        AdvanceTurn();
    }

    private IEnumerator RunEnemyAttacksPhase()
    {
        yield return StartCoroutine(_cardsPointController.RunEnemyAttacks());
        AdvanceTurn();
    }

    private void EndBattle()
    {
        
    }
}
