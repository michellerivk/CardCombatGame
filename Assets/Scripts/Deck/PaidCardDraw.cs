using UnityEngine;

public class PaidCardDraw : MonoBehaviour
{
    [SerializeField] private DeckController _deck; 
    [SerializeField] private BattleController _battleController; 
    [SerializeField] private ManaPool _mana;
    [SerializeField] private int _cost = 2;

    public bool CanDraw => _battleController.CurrentPhase == TurnOrder.playerActive && 
                           _deck.CanDraw && _mana.CanAfford(_cost);

    public void TryDraw()
    {
        if (!_deck.CanDraw || !_mana.TrySpend(_cost) || _battleController.CurrentPhase != TurnOrder.playerActive)
            return;

        _deck.DrawCardsToHand(1);
    }
}
