using UnityEngine;

public class PaidCardDraw : MonoBehaviour
{
    [SerializeField] private DeckController _deck; 
    [SerializeField] private BattleController _battleController; 
    [SerializeField] private ManaPool _mana;
    [SerializeField] private int _cost = 2;

    public bool CanDraw => !_battleController.IsBattleOver &&
                           _battleController.CurrentPhase == TurnOrder.playerActive &&
                           _deck.CanDraw &&
                           _mana.CanAfford(_cost);

    public void TryDraw()
    {
        // Validate every rule before spending mana or starting a draw.
        if (!CanDraw)
            return;

        if (_mana.TrySpend(_cost))
            _deck.DrawCardsToHand(1);
    }
}
