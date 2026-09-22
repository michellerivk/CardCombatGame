using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Definition")]
    [SerializeField] private CardSO _cardSO;

    private int _currentHealth, _attackPower, _manaCost;
    private bool _isDefeated;

    public int CurrentHealth => _currentHealth;
    public int AttackPower => _attackPower;
    public int ManaCost => _manaCost;
    public bool IsDefeated => _isDefeated;

    public string CardName => _cardSO.cardName;
    public string CardDescription => _cardSO.actionDescription;
    public string CardLore => _cardSO.cardLore;
    public Sprite CardCharacter => _cardSO.characterSprite;
    public Sprite CardBG => _cardSO.bgSprite;

    public event Action OnChanged, OnDamage, OnAttack;
    public event Action<Card> OnDefeated;

    private void Awake()
    {
        SetupCardData();
    }

    private void SetupCardData()
    {
        _isDefeated = false;
        _currentHealth = _cardSO.currentHealth;
        _attackPower = _cardSO.attackPower;
        _manaCost = _cardSO.manaCost;
    }

    public void Initialize(CardSO definition)
    {
        if (definition == null)
        {
            Debug.LogError("Cannot initialize a card without a CardSO.", this);
            return;
        }

        _cardSO = definition;
        SetupCardData();    
        OnChanged?.Invoke();
    }

    public void NotifyAttack()
    {
        if (_isDefeated)
            return;

        OnAttack?.Invoke();
    }

    public void DamageCard(int damage)
    {
        if (_isDefeated || damage <= 0)
            return;

        OnDamage?.Invoke();

        SetCurrentHealth(CurrentHealth - damage);
    }
    private void SetCurrentHealth(int health)
    {
        if (_isDefeated || _currentHealth == health) return;

        _currentHealth = Mathf.Max(0, health);
        _isDefeated = _currentHealth == 0;
        OnChanged?.Invoke();

        if (_isDefeated)
            OnDefeated?.Invoke(this);
    }
}
