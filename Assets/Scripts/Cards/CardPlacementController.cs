using UnityEngine;

public class CardPlacementController : MonoBehaviour
{
    [SerializeField] private Card _card;
    [SerializeField] private CardPointerInput _input;
    [SerializeField] private CardSelectionController _selection;
    [SerializeField] private CardHandState _handState;
    [SerializeField] private CardMotion _motion;
    [SerializeField] private LayerMask _whatIsPlacement;

    private Camera _mainCamera;
    private int _selectedFrame = -1;
    private ManaPool _mana;
    private HandController _handController;
    private BattleController _battleController;
    private bool _listeningToBattle;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _input.OnPlacementInteraction += TryPlaceCard;
        _selection.OnSelectionChanged += HandleSelectionChanged;
        SubscribeToBattle();
    }

    private void OnDisable()
    {
        _input.OnPlacementInteraction -= TryPlaceCard;
        _selection.OnSelectionChanged -= HandleSelectionChanged;
        UnsubscribeFromBattle();
    }

    public void Initialize(
        HandController handController,
        ManaPool mana,
        BattleController battleController)
    {
        if (handController == null || mana == null || battleController == null)
        {
            Debug.LogError(
                $"{name} received invalid player dependencies.",
                this);

            return;
        }

        UnsubscribeFromBattle();
        _handController = handController;
        _mana = mana;
        _battleController = battleController;
        SubscribeToBattle();
    }

    private void HandleSelectionChanged(bool isSelected)
    {
        if (isSelected && !CanPlayerInteract())
        {
            _selection.UnSelectCard();
            return;
        }

        _selectedFrame = isSelected ? Time.frameCount : -1;
    }

    private void TryPlaceCard(Vector2 pointerPosition)
    {
        if (!_selection.IsSelected || Time.frameCount == _selectedFrame)
            return;

        // This must happen before raycasting or reserving a board slot.
        if (!CanPlayerInteract())
        {
            ReturnToHand();
            return;
        }

        if (_mainCamera == null)
        {
            Debug.LogError("Card placement needs a Main Camera.", this);
            ReturnToHand();
            return;
        }

        if (_handController == null || _mana == null || _battleController == null)
        {
            Debug.LogError(
                $"{name} has not been initialized by PlayerBattleContext.",
                this);

            ReturnToHand();
            return;
        }

        Ray ray = _mainCamera.ScreenPointToRay(pointerPosition);

        if (!Physics.Raycast(ray,out RaycastHit hit, 100f, _whatIsPlacement))
        {
            ReturnToHand();
            return;
        }

        CardPlacePoint selectedPoint =
            hit.collider.GetComponentInParent<CardPlacePoint>();

        if (selectedPoint == null || !selectedPoint.IsPlayerPoint)
        {
            ReturnToHand();
            return;
        }

        // Recheck every rule immediately before the only operation that reserves a slot.
        if (!CanPlayerInteract() || !_mana.CanAfford(_card.ManaCost) ||
            !selectedPoint.TryAssign(_card))
        {
            ReturnToHand();
            return;
        }

        if (!_mana.TrySpend(_card.ManaCost))
        {
            selectedPoint.Release(_card);
            ReturnToHand();
            return;
        }

        PlaceCard(selectedPoint);
    }

    private void PlaceCard(CardPlacePoint selectedPoint)
    {
        if (!_handController.RemoveCardFromHand(_handState))
        {
            selectedPoint.Release(_card);
            ReturnToHand();
            return;
        }

        _selection.UnSelectCard();

        _motion.SetTargetPose(
            selectedPoint.transform.position,
            Quaternion.identity);
    }

    private void ReturnToHand()
    {
        _selection.UnSelectCard();
    }

    private bool CanPlayerInteract()
    {
        return _battleController != null &&
               !_battleController.IsBattleOver &&
               _battleController.CurrentPhase == TurnOrder.playerActive;
    }

    private void SubscribeToBattle()
    {
        if (_listeningToBattle || _battleController == null || !isActiveAndEnabled)
            return;

        _battleController.OnPhaseChanged += HandlePhaseChanged;
        _battleController.OnBattleEnded += HandleBattleEnded;
        _listeningToBattle = true;
    }

    private void UnsubscribeFromBattle()
    {
        if (!_listeningToBattle || _battleController == null)
            return;

        _battleController.OnPhaseChanged -= HandlePhaseChanged;
        _battleController.OnBattleEnded -= HandleBattleEnded;
        _listeningToBattle = false;
    }

    private void HandlePhaseChanged(TurnOrder phase)
    {
        if (phase != TurnOrder.playerActive)
            ReturnToHand();
    }

    private void HandleBattleEnded(BattleResult result)
    {
        ReturnToHand();
    }
}
