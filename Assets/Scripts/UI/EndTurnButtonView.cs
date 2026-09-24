using UnityEngine;

public class EndTurnButtonView : MonoBehaviour
{
    [SerializeField] private BattleController _battle;
    [SerializeField] private CanvasGroup _visual;

    private void OnEnable()
    {
        _battle.OnPhaseChanged += Refresh;
        _battle.OnBattleEnded += DisableButton;
    }

    private void Start()
    {
        Refresh(_battle.CurrentPhase);
    }

    private void OnDisable()
    {
        _battle.OnPhaseChanged -= Refresh;
        _battle.OnBattleEnded -= DisableButton;
    }

    private void Refresh(TurnOrder phase)
    {
        bool visible = !_battle.IsBattleOver && phase == TurnOrder.playerActive;
        _visual.alpha = visible ? 1f : 0f;
        _visual.interactable = visible;
        _visual.blocksRaycasts = visible;
    }

    // Maybe I should get the event from the UI controller in the future instead of the battle controller?
    private void DisableButton(BattleResult result)
    {
        _visual.alpha = 0f;
        _visual.interactable = false;
        _visual.blocksRaycasts = false;
    }

    public void EndTurnClicked()
    {
        _battle.EndPlayerTurn();
    }
}
