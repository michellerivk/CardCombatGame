using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("SO")]
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

    public event Action OnSetup;

    void Start()
    {
        SetupCardData();
    }

    private void SetupCardData()
    {
        _currentHealth = _cardSO.currentHealth;
        _attackPower = _cardSO.attackPower;
        _manaCost = _cardSO.manaCost;

        OnSetup?.Invoke();
    }
}
