using UnityEngine;

public class CardHoverAnimator : MonoBehaviour
{
    [SerializeField] private Card _card;
    [SerializeField] private CardDealingAnimator _cardDealingAnimator;

    private Vector3 _returnPosition;
    private Quaternion _returnRotation;


    void OnEnable()
    {
        _card.OnHover += HoverCard;
    }
    void OnDisable()
    {
        _card.OnHover -= HoverCard;
    }

    private void HoverCard(bool shouldHover)
    {
        if (shouldHover == true)
        {
            _returnPosition = _card.transform.position;
            _returnRotation = _card.transform.rotation;

            _cardDealingAnimator.MoveToPoint(
                           _cardDealingAnimator.HandPosition + new Vector3(0f, 1f, 0.5f), 
                            Quaternion.identity);
        }
        else
        {
             _cardDealingAnimator.MoveToPoint(
                            _cardDealingAnimator.HandPosition,
                            _cardDealingAnimator.HandRotation);
        }
    }

}
