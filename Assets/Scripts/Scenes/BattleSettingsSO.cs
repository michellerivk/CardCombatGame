using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/Battle Settings")]
public class BattleSettingsSO : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string _battleName = "Battle";

    [Header("Visuals")]
    [SerializeField] private Material _deskMaterial;
    [SerializeField] private Sprite _background;

    [Header("Enemy")]
    [SerializeField] private EnemyAI.AIType _enemyAIType;
    [SerializeField] private List<CardSO> _enemyDeck = new List<CardSO>();
    [SerializeField, Min(1)] private int _enemyStartingHealth = 30;
    [SerializeField, Min(0)] private int _enemyStartingMana = 4;
    [SerializeField, Min(0)] private int _enemyOpeningHandSize = 5;
    [SerializeField, Min(0)] private int _enemyCardsPlayedPerTurn = 5;

    [Header("Player")]
    [SerializeField] private List<CardSO> _playerDeck = new List<CardSO>();
    [SerializeField, Min(1)] private int _playerStartingHealth = 30;
    [SerializeField, Min(0)] private int _playerStartingMana = 4;

    [Header("Rules")]
    [SerializeField, Min(0)] private int _openingHandSize = 5;
    [SerializeField, Min(0)] private int _cardsDrawnPerTurn = 1;
    [SerializeField, Min(0)] private int _maximumMana = 12;

    public string BattleName => _battleName;
    public Material DeskMaterial => _deskMaterial;
    public Sprite Background => _background;
    public EnemyAI.AIType EnemyAIType => _enemyAIType;
    public IReadOnlyList<CardSO> EnemyDeck => _enemyDeck;
    public int EnemyStartingHealth => _enemyStartingHealth;
    public int EnemyStartingMana => _enemyStartingMana;
    public int EnemyOpeningHandSize => _enemyOpeningHandSize;
    public int EnemyCardsPlayedPerTurn => _enemyCardsPlayedPerTurn;
    public IReadOnlyList<CardSO> PlayerDeck => _playerDeck;
    public int PlayerStartingHealth => _playerStartingHealth;
    public int PlayerStartingMana => _playerStartingMana;
    public int OpeningHandSize => _openingHandSize;
    public int CardsDrawnPerTurn => _cardsDrawnPerTurn;
    public int MaximumMana => _maximumMana;
}
