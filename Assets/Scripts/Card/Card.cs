using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CardSO _cardSO;
    [SerializeField] private HandController _handController;

    [Header("Parameters")]
    private bool _isInHand = false;
    private int _handPosition;

    private int _currentHealth, _attackPower, _manaCost;

    public int CurrentHealth => _currentHealth;
    public int AttackPower => _attackPower;
    public int ManaCost => _manaCost;

    public string CardName => _cardSO.cardName;
    public string CardDescription => _cardSO.actionDescription;
    public string CardLore => _cardSO.cardLore;
    public Sprite CardCharacter => _cardSO.characterSprite;
    public Sprite CardBG => _cardSO.bgSprite;

    public event Action OnChanged; // Later when the card will take damage 
    public event Action<bool> OnHover;

    void Awake()
    {
        SetupCardData();
    }

    void OnMouseEnter()
    {
        if (_isInHand)
        {
            OnHover?.Invoke(true);
        }

    }

    void OnMouseExit()
    {
        if (_isInHand)
        {
            OnHover?.Invoke(false);
        }
    }

    private void SetupCardData()
    {
        _currentHealth = _cardSO.currentHealth;
        _attackPower = _cardSO.attackPower;
        _manaCost = _cardSO.manaCost;
    }

    void OnEnable()
    {
        _handController.OnCardHeldChanged += AddOrRemoveCardFromHand;
    }

    void OnDisable()
    {
        _handController.OnCardHeldChanged -= AddOrRemoveCardFromHand;
    }

    private void AddOrRemoveCardFromHand(Card card, bool isInHand, int handPosition)
    {
         if (card != this) return;

        _isInHand = isInHand;
        _handPosition = handPosition;
    }
}
