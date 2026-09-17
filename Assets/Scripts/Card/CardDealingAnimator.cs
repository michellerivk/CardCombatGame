using UnityEngine;
using UnityEngine.XR;

public class CardDealingAnimator : MonoBehaviour
{
    [SerializeField] private HandController _handController;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 540f;
    private Vector3 _targetPoint;
    private Quaternion _targetRot;
    private Card _card;

    public Vector3 HandPosition { get; private set; }
    public Quaternion HandRotation { get; private set; }


    private void Awake()
    {
        _card = GetComponent<Card>();
    }

    private void OnEnable()
    {
        _handController.OnCardDealt += HandleCardDealt;
    }

    private void OnDisable()
    {
        _handController.OnCardDealt -= HandleCardDealt;
    }


    // TODO: Should replace with a better method
    void Update()
    {
        if(transform.position == _targetPoint) return;

        transform.position = Vector3.Lerp(transform.position, _targetPoint, _moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRot, _rotateSpeed * Time.deltaTime);
    }

    private void HandleCardDealt(Card card, Vector3 position,  Quaternion rotation)
    {
        if (card != _card) return;

        HandPosition = position;
        HandRotation = rotation;

        MoveToPoint(position, rotation);
    }
    public void MoveToPoint(Vector3 point, Quaternion rotToMatch)
    {
        _targetPoint = point;
        _targetRot = rotToMatch;

        
    }
}
