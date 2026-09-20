using UnityEngine;

public class CardHandState : MonoBehaviour
{
    public bool IsInHand { get; private set; }
    public int HandPosition { get; private set; } = -1;

    public void PlaceAt(int position)
    {
        IsInHand = true;
        HandPosition = position;
    }

    public void RemoveFromHand()
    {
        IsInHand = false;
        HandPosition = -1;
    }
}
