using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private List<Card> heldCards = new List<Card>();
    [SerializeField] private Transform minPos, maxPos;

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
}
