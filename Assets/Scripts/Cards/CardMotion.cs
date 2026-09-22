using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardMotion : MonoBehaviour
{
    [SerializeField] private CardSelectionController _cardSelectionCR;
    [SerializeField] private CardHandState _handState;

    [SerializeField] private LayerMask _whatIsDesktop;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 540f;
    [SerializeField] private Vector3 _hoverOffset = new Vector3(0f, 1f, 0.5f);
    [SerializeField] private float _selectedHeight = 1f;

    private Vector3 _targetPoint;
    private Quaternion _targetRot;
    private bool _hasHandPose;
    private bool _isHovered;
    private bool _isSelected;
    private bool _hasExplicitTarget;
    private Action _onArrived;

    private Ray _ray;
    private RaycastHit _hit;

    public Vector3 HandPosition { get; private set; }
    public Quaternion HandRotation { get; private set; }

    void Awake()
    {
        if (_cardSelectionCR == null) _cardSelectionCR = GetComponent<CardSelectionController>();
        if (_handState == null) _handState = GetComponent<CardHandState>();
    }

    private void OnEnable()
    {
        if (_cardSelectionCR != null)
            _cardSelectionCR.OnSelectionChanged += SetSelected;
    }

    private void OnDisable()
    {
        if (_cardSelectionCR != null)
            _cardSelectionCR.OnSelectionChanged -= SetSelected;
    }

    private void Update()
    {
        if (!_hasHandPose && !_hasExplicitTarget)
            return;

        if (!_hasExplicitTarget)
            UpdateTarget();

        bool arrived = MoveToTarget(_targetPoint, _targetRot);
        if (_hasExplicitTarget && arrived)
        {
            // Clear first: the callback may request another move.
            Action onArrived = _onArrived;
            _onArrived = null;
            onArrived?.Invoke();
        }
    }

    public void SetHandPose(Vector3 position, Quaternion rotation)
    {
        HandPosition = position;
        HandRotation = rotation;
        _hasHandPose = true;
        if (!_hasExplicitTarget)
            UpdateTarget();
    }

    public void SetHovered(bool isHovered)
    {
        if (!_hasHandPose || _isHovered == isHovered) return;

        _isHovered = isHovered;
    }

    private void SetSelected(bool isSelected)
    {
        if (_isSelected == isSelected)
            return;

        _isSelected = isSelected;

        if (!isSelected)
            _isHovered = false;
    }

    private void UpdateTarget()
    {
        // Explicit moves take priority in Update over selection and hand targets.
        if (_isSelected)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            _ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(_ray, out _hit, 100f, _whatIsDesktop))
            {
                _targetPoint = _hit.point + _hit.normal * _selectedHeight;
                _targetRot = Quaternion.identity;
            }

            // Preserve the last valid mouse target if the ray misses.
            return;
        }

        // A card on the board keeps the target supplied by SetTargetPose().
        if (_handState == null || !_handState.IsInHand)
            return;

        // Otherwise use the hover or normal hand pose.
        _targetPoint = HandPosition +  (_isHovered ? _hoverOffset : Vector3.zero);

        _targetRot = _isHovered ? Quaternion.identity : HandRotation;
    }

    private bool MoveToTarget(Vector3 position, Quaternion rotation)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            position,
            _moveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            rotation,
            _rotateSpeed * Time.deltaTime);

        bool arrived = (transform.position - position).sqrMagnitude <= 0.0001f &&
                       Quaternion.Angle(transform.rotation, rotation) <= 0.5f;
        if (arrived)
            transform.SetPositionAndRotation(position, rotation);

        return arrived;
    }

    public void SetTargetPose(Vector3 position, Quaternion rotation)
    {
        MoveTo(position, rotation);
    }

    // Replaces any pending move; holds the pose after arrival until resumed or redirected.
    public void MoveTo(Vector3 position, Quaternion rotation, Action onArrived = null)
    {
        _targetPoint = position;
        _targetRot = rotation;
        _onArrived = onArrived;
        _hasExplicitTarget = true;
    }

    public void ResumeHandMotion()
    {
        _hasExplicitTarget = false;
        _onArrived = null;
    }
}
