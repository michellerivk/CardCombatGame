using System;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Definition")]
    [SerializeField] private CardSO _cardSO;

    private int _currentHealth, _attackPower, _manaCost;
    private bool _isDefeated;
    private readonly List<CardAbility> _abilities = new List<CardAbility>();

    public int CurrentHealth => _currentHealth;
    public int AttackPower => _attackPower;
    public int ManaCost => _manaCost;
    public bool IsDefeated => _isDefeated;
    public IReadOnlyList<CardAbility> Abilities => _abilities;

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

        ClearAbilities();
        if (_cardSO.abilities != null)
        {
            foreach (AbilitySO definition in _cardSO.abilities)
                AddAbility(definition, false);
        }
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

    public bool HasAbility(AbilitySO definition)
    {
        if (definition == null)
            return false;

        foreach (CardAbility ability in _abilities)
        {
            if (ability.Definition == definition)
                return true;
        }

        return false;
    }

    public bool HasAbility<TAbility>() where TAbility : CardAbility
    {
        foreach (CardAbility ability in _abilities)
        {
            if (ability is TAbility)
                return true;
        }

        return false;
    }

    public bool AddAbility(AbilitySO definition)
    {
        return AddAbility(definition, true);
    }

    private bool AddAbility(AbilitySO definition, bool notifyChanged)
    {
        if (definition == null || HasAbility(definition))
            return false;

        CardAbility ability = definition.CreateRuntimeAbility();
        return AddAbility(ability, notifyChanged);
    }

    public bool AddAbility(CardAbility ability)
    {
        return AddAbility(ability, true);
    }

    private bool AddAbility(CardAbility ability, bool notifyChanged)
    {
        if (ability == null || HasAbility(ability.Definition) || !ability.TryAttachTo(this))
            return false;

        int insertionIndex = _abilities.Count;
        while (insertionIndex > 0 && _abilities[insertionIndex - 1].Priority > ability.Priority)
            insertionIndex--;

        _abilities.Insert(insertionIndex, ability);

        if (notifyChanged)
            OnChanged?.Invoke();

        return true;
    }

    public bool TryRemoveAbility(AbilitySO definition, out CardAbility removedAbility)
    {
        for (int i = 0; i < _abilities.Count; i++)
        {
            if (_abilities[i].Definition != definition)
                continue;

            removedAbility = _abilities[i];
            _abilities.RemoveAt(i);
            removedAbility.DetachFrom(this);
            OnChanged?.Invoke();
            return true;
        }

        removedAbility = null;
        return false;
    }

    public bool TryRemoveAbility<TAbility>(out TAbility removedAbility)
        where TAbility : CardAbility
    {
        for (int i = 0; i < _abilities.Count; i++)
        {
            if (!(_abilities[i] is TAbility matchingAbility))
                continue;

            _abilities.RemoveAt(i);
            matchingAbility.DetachFrom(this);
            removedAbility = matchingAbility;
            OnChanged?.Invoke();
            return true;
        }

        removedAbility = null;
        return false;
    }

    private void ClearAbilities()
    {
        foreach (CardAbility ability in _abilities)
            ability.DetachFrom(this);

        _abilities.Clear();
    }

    internal void NotifyAbilityStateChanged()
    {
        OnChanged?.Invoke();
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
