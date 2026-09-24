using UnityEngine;

public class PlayerBattleContext : MonoBehaviour
{
    [SerializeField] private ManaPool _mana;
    [SerializeField] private HandController _hand;
    [SerializeField] private BattleController _battle;

    private void Awake()
    {
        foreach (Card card in _hand.HeldCards)
            InitializeCard(card);
    }

    private bool InitializeCard(Card card)
    {
        if (card == null)
            return false;

        if (!card.TryGetComponent(out CardPlacementController placement))
        {
            Debug.LogError(
                $"{card.name} needs CardPlacementController.", card);

            return false;
        }

        placement.Initialize(_hand, _mana, _battle);
        return true;
    }

    public void AddCardToHand(Card card)
    {
        if (!card.TryGetComponent(out CardPlacementController placement))
        {
            Debug.LogError($"{card.name} needs CardPlacementController.", card);

            return;
        }

        placement.Initialize(_hand, _mana, _battle);
        _hand.AddCard(card);
    }
}
