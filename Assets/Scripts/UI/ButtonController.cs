using System;
using TMPro;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private PaidCardDraw _paidDraw;
    [SerializeField] private ManaPool _manaPool;
    [SerializeField] private BattleController _battleController;
    [SerializeField] private CanvasGroup _buttonVisual;


    private void OnEnable()
    {
        _manaPool.OnManaChanged += HandleManaChanged;
        _battleController.OnPhaseChanged += HandlePhaseChanged;
    }

    private void Start()
    {
        RefreshButton();
    }

    private void OnDisable()
    {
        _manaPool.OnManaChanged -= HandleManaChanged;
        _battleController.OnPhaseChanged -= HandlePhaseChanged;
    }

    public void DrawButtonClicked()
    {
        _paidDraw.TryDraw();
        RefreshButton();
    }
    private void HandleManaChanged(int current, int maximum) => RefreshButton();
    private void HandlePhaseChanged(TurnOrder phase) => RefreshButton();

    private void RefreshButton()
    {
        bool canDraw = _paidDraw.CanDraw;
        _buttonVisual.alpha = canDraw ? 1f : 0f;
        _buttonVisual.interactable = canDraw;
        _buttonVisual.blocksRaycasts = canDraw;
    }
}
