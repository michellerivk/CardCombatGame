using UnityEngine;

public class CardPointerInput : MonoBehaviour
{
    [SerializeField] private CardHandState _handState;
    [SerializeField] private CardMotion _motion;

    private void Awake()
    {
        _handState ??= GetComponent<CardHandState>();
        _motion ??= GetComponent<CardMotion>();
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
}
