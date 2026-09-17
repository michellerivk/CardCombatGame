using UnityEngine;

public class CardMotion : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 540f;
    [SerializeField] private Vector3 _hoverOffset = new Vector3(0f, 1f, 0.5f);

    private Vector3 _targetPoint;
    private Quaternion _targetRot;
    private bool _hasHandPose;
    private bool _isHovered;

    public Vector3 HandPosition { get; private set; }
    public Quaternion HandRotation { get; private set; }

    private void Update()
    {
        if (!_hasHandPose) return;

        transform.position = Vector3.MoveTowards(transform.position, _targetPoint, _moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRot, _rotateSpeed * Time.deltaTime);
    }

    public void SetHandPose(Vector3 position, Quaternion rotation)
    {
        HandPosition = position;
        HandRotation = rotation;
        _hasHandPose = true;
        UpdateTarget();
    }

    public void SetHovered(bool isHovered)
    {
        if (!_hasHandPose || _isHovered == isHovered) return;

        _isHovered = isHovered;
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        _targetPoint = HandPosition + (_isHovered ? _hoverOffset : Vector3.zero);
        _targetRot = _isHovered ? Quaternion.identity : HandRotation;
    }
}
