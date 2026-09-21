using UnityEngine;

public class EndTurnButtonView : MonoBehaviour
{
    [SerializeField] private BattleController _battle;
    [SerializeField] private CanvasGroup _visual;

    private void OnEnable()
    {
        _battle.OnPhaseChanged += Refresh;
    }

    private void Start()
    {
        Refresh(_battle.CurrentPhase);
    }

    private void OnDisable()
    {
        _battle.OnPhaseChanged -= Refresh;
    }

    private void Refresh(TurnOrder phase)
    {
        bool visible = phase == TurnOrder.playerActive;
        _visual.alpha = visible ? 1f : 0f;
        _visual.interactable = visible;
        _visual.blocksRaycasts = visible;
    }

    public void EndTurnClicked()
    {
        _battle.EndPlayerTurn();
    }
}