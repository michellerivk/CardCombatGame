using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Definition")]
    [SerializeField] private CardSO _cardSO;

    private int _currentHealth, _attackPower, _manaCost;

    public int CurrentHealth => _currentHealth;
    public int AttackPower => _attackPower;
    public int ManaCost => _manaCost;

    public string CardName => _cardSO.cardName;
    public string CardDescription => _cardSO.actionDescription;
    public string CardLore => _cardSO.cardLore;
    public Sprite CardCharacter => _cardSO.characterSprite;
    public Sprite CardBG => _cardSO.bgSprite;

    public event Action OnChanged;
    public event Action<Card> OnDefeated;

    private void Awake()
    {
        SetupCardData();
    }

    private void SetupCardData()
    {
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

    public void DamageCard(int damage)
    {
        SetCurrentHealth(CurrentHealth - damage);
    }
    private void SetCurrentHealth(int health)
    {
        if (_currentHealth == health) return;

        _currentHealth = health;

        if (_currentHealth <= 0) 
        {
            _currentHealth = 0;
            OnDefeated?.Invoke(this);
            Destroy(gameObject, 5f);
        }

        OnChanged?.Invoke();
    }
}
