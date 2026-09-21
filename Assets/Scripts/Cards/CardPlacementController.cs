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

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _input.OnPlacementInteraction += TryPlaceCard;
        _selection.OnSelectionChanged += HandleSelectionChanged;
    }

    private void OnDisable()
    {
        _input.OnPlacementInteraction -= TryPlaceCard;
        _selection.OnSelectionChanged -= HandleSelectionChanged;
    }

    public void Initialize(HandController handController, ManaPool mana)
    {
        if (handController == null || mana == null)
        {
            Debug.LogError(
                $"{name} received invalid player dependencies.",
                this);

            return;
        }

        _handController = handController;
        _mana = mana;
    }

    private void HandleSelectionChanged(bool isSelected)
    {
        _selectedFrame = isSelected ? Time.frameCount : -1;
    }

    private void TryPlaceCard(Vector2 pointerPosition)
    {
        if (!_selection.IsSelected || Time.frameCount == _selectedFrame)
            return;

        if (_mainCamera == null || _handController == null)
        {
            Debug.LogError("Card placement needs a Main Camera and a HandController.", this);
            ReturnToHand();
            return;
        }

        if (_handController == null || _mana == null)
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

        if (!_mana.TrySpend(_card.ManaCost))
            return;

        if (selectedPoint == null ||
            !selectedPoint.TryAssign(_card))
        {
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
}
