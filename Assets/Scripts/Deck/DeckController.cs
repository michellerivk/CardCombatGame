using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeckController : MonoBehaviour
{
    [SerializeField] private List<CardSO> _deckToUse = new List<CardSO>(); 
    [SerializeField] private Card _cardToSpawn;
    [SerializeField] private PlayerBattleContext _playerContext;
    [SerializeField] private float waitBetweenDrawingCards = 0.25f;

    public bool CanDraw => _activeCards.Count > 0 || _deckToUse.Count > 0;

    private List<CardSO> _activeCards = new List<CardSO>(); 

    public List<CardSO> DeckCards => _deckToUse;
    public List<CardSO> ActiveCards => _activeCards;

    private void Awake()
    {
        SetupDeck();
    }

    private void SetupDeck()
    {
        _activeCards.Clear();

        List<CardSO> tempDeck = new List<CardSO>();
        tempDeck.AddRange(_deckToUse);

        int selected;
        //int iterations = 0; // Only for creating a deck with duplicates

        // Inserting the possible SOs into a temp deck
        while(tempDeck.Count > 0) //&& iterations < 500) // Only for creating a deck with duplicates
        {
            selected = Random.Range(0, tempDeck.Count);
            _activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);

            //iterations ++; // Only for creating a deck with duplicates
        }
    }

    private bool TryDrawCardToHand()
    {
        if (_activeCards.Count == 0)
            SetupDeck();

        if (_activeCards.Count == 0) // The deck is empty
            return false;

        Card newCard = Instantiate(_cardToSpawn, transform.position, transform.rotation);
        newCard.Initialize(_activeCards[0]);

        _playerContext.AddCardToHand(newCard);

        _activeCards.RemoveAt(0);
        return true;
    }

    public void DrawCardsToHand(int count)
    {
        StartCoroutine(DrawCardsToHandCo(count));
    }

    private IEnumerator DrawCardsToHandCo(int amountToDraw)
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            TryDrawCardToHand();
            yield return new WaitForSeconds(waitBetweenDrawingCards);
        }
    }
}
