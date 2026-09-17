using System;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private List<Card> heldCards = new List<Card>();
    [SerializeField] private Transform minPos, maxPos;

    private List<Vector3> cardPositions = new List<Vector3>();

    public event Action<Card, Vector3, Quaternion> OnCardDealt;
    public event Action<Card, bool, int> OnCardHeldChanged;


    void Start()
    {
        SetCardPositionsInHand();
    }

    private void SetCardPositionsInHand()
    {
        cardPositions.Clear();

        Vector3 distanceBetweenPoints = Vector3.zero;

        if(heldCards.Count > 1)
        {
            distanceBetweenPoints = (maxPos.position - minPos.position) / 
                                    (heldCards.Count - 1);
        }

        for(int i = 0; i < heldCards.Count; i++)
        {
            cardPositions.Add(minPos.position + (distanceBetweenPoints * i));

            //heldCards[i].transform.position = cardPositions[i];
            //heldCards[i].transform.rotation = minPos.rotation;

           OnCardDealt?.Invoke(heldCards[i], cardPositions[i], minPos.rotation);
           OnCardHeldChanged?.Invoke(heldCards[i], true, i);
        }
    }
}
