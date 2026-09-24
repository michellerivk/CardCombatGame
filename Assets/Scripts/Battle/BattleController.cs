using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnOrder {playerActive, playerCardAttacks, enemyActive, enemyCardAttacks}
public enum BattleResult { Victory, Defeat }
public class BattleController : MonoBehaviour
{
    [Header("Mana")]
    [SerializeField] private ManaPool _playerMana;
    [SerializeField] private ManaPool _enemyMana;

    [Header("Health")]
    [SerializeField] private HealthPool _playerHealth;
    [SerializeField] private HealthPool _enemyHealth;

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
    [SerializeField] private HandController _playerHand;
    [SerializeField] private CardDiscardController _discardController;

    public bool IsBattleOver { get; private set; }

    // This variable can hold a BattleResult value or null
    public BattleResult? Result { get; private set; }
    public TurnOrder CurrentPhase => _currentPhase;
    public event Action<TurnOrder> OnPhaseChanged; // CardPlacePoint listens
    public event Action<BattleResult> OnBattleEnded;



    private void Start()
    {
        _playerDeck.DrawCardsToHand(_openingHandSize);
    }

    public void Initialize(int openingHandSize, int cardsToDrawPerTurn)
    {
        _openingHandSize = Mathf.Max(0, openingHandSize);
        _cardsToDrawPerTurn = Mathf.Max(0, cardsToDrawPerTurn);
        _currentPhase = TurnOrder.playerActive;
        IsBattleOver = false;
        Result = null;
    }

    void OnEnable()
    {
        _playerHealth.OnDied += HandlePlayerDied;
        _enemyHealth.OnDied += HandleEnemyDied;
    }

    void OnDisable()
    {
        _playerHealth.OnDied -= HandlePlayerDied;
        _enemyHealth.OnDied -= HandleEnemyDied;
    }


    private void AdvanceTurn()
    {
        if (IsBattleOver) return;

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
        if (IsBattleOver) return;

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

    private void HandlePlayerDied()
    {
        EndBattle(BattleResult.Defeat);
    }
    private void HandleEnemyDied()
    {
        EndBattle(BattleResult.Victory);
    }

    private void EndBattle(BattleResult result)
    {
        if (IsBattleOver)
            return;

        // Set the state before notifying anyone.
        IsBattleOver = true;
        StopAllCoroutines();

        // Stop systems that own coroutines or card collections of their own.
        _playerDeck.CancelPendingDraws();
        var cardsFromHand = _playerHand.EmptyHand();
        StartCoroutine(FinishBattle(result, cardsFromHand));
    }

    private IEnumerator FinishBattle(BattleResult result, IReadOnlyList<Card> cardsFromHand)
    {
        yield return _discardController.DiscardAllCardsAndWait(cardsFromHand);

        Result = result;
        OnBattleEnded?.Invoke(result);
    }
}
