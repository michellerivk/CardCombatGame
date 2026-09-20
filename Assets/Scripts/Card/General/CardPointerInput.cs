using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardPointerInput : MonoBehaviour
{
    [SerializeField] private CardHandState _handState;
    [SerializeField] private CardMotion _motion;

    private Mouse _mouse;
    private Vector2 _pointerPosition;

    public event Action OnSelect;
    public event Action OnCancelSelection;
    public event Action<Vector2> OnPlacementInteraction;


    private void Awake()
    {
        _handState ??= GetComponent<CardHandState>();
        _motion ??= GetComponent<CardMotion>();
    }
    private void Update()
    {
        _mouse = Mouse.current;

        if (_mouse == null)
            return;

        if (_mouse.rightButton.wasPressedThisFrame)
            OnCancelSelection?.Invoke();

        if (_mouse.leftButton.wasPressedThisFrame)
        {
            _pointerPosition = _mouse.position.ReadValue();
            OnPlacementInteraction?.Invoke(_pointerPosition);
        }

    }

    private void OnMouseEnter()
    {
        if (_handState.IsInHand)
            _motion.SetHovered(true);
    }

    private void OnMouseExit()
    {
        _motion.SetHovered(false);
    }

    private void OnDisable()
    {
        _motion.SetHovered(false);
    }

    private void OnMouseDown()
    {
        if (_handState.IsInHand)
        {
            OnSelect?.Invoke();
        }
    }
}
