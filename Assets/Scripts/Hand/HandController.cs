using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private List<Card> heldCards = new List<Card>();
    [SerializeField] private Transform minPos, maxPos;

    public IReadOnlyList<Card> HeldCards => heldCards;

    private void Start()
    {
        SetCardPositionsInHand();
    }

    private void SetCardPositionsInHand()
    {
        Vector3 distanceBetweenPoints = Vector3.zero;
        if (heldCards.Count > 1)
            distanceBetweenPoints = (maxPos.position - minPos.position) / (heldCards.Count - 1);

        for (int i = 0; i < heldCards.Count; i++)
        {
            Card card = heldCards[i];
            if (!card.TryGetComponent(out CardHandState handState) ||
                !card.TryGetComponent(out CardMotion motion))
            {
                Debug.LogError($"{card.name} needs CardHandState and CardMotion components.", card);
                continue;
            }

            handState.PlaceAt(i);
            motion.SetHandPose(minPos.position + distanceBetweenPoints * i, minPos.rotation);
        }
    }

    public bool RemoveCardFromHand(CardHandState cardToRemove)
    {
        if (cardToRemove == null || !cardToRemove.TryGetComponent(out Card card))
        {
            Debug.LogError("The card being removed needs Card and CardHandState components.");
            return false;
        }

        int cardIndex = heldCards.IndexOf(card);
        if (cardIndex < 0)
        {
            Debug.LogError($"{card.name} is not registered in this hand.", card);
            return false;
        }

        heldCards.RemoveAt(cardIndex);
        cardToRemove.RemoveFromHand();
        SetCardPositionsInHand();
        return true;
    }

    public void AddCard(Card card)
    {
        if (card == null || heldCards.Contains(card))
            return;

        heldCards.Add(card);
        SetCardPositionsInHand();
    }
}
