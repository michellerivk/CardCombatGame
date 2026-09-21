using System;
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

    public TurnOrder CurrentPhase => _currentPhase;
    public event Action<TurnOrder> OnPhaseChanged;


    private void Start()
    {
        _playerDeck.DrawCardsToHand(_openingHandSize);
    }

    private void AdvanceTurn()
    {
        _currentPhase++;
        
        if((int)_currentPhase >= Enum.GetValues(typeof(TurnOrder)).Length)
            _currentPhase = 0;

        switch (_currentPhase)
        {
            case TurnOrder.playerActive:
                _playerMana.UpdatePlayerMana();
                _playerDeck.DrawCardsToHand(_cardsToDrawPerTurn);
                break;

            case TurnOrder.playerCardAttacks:
                AdvanceTurn();
                break;

            case TurnOrder.enemyActive:
                AdvanceTurn();
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
        OnPhaseChanged?.Invoke(_currentPhase);
    }

    private void StartPlayerTurn()
    {
        //_playerMana.RefillForNewTurn();
    }
}
