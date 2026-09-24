using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    [SerializeField] private List<CardSO> _deckToUse = new List<CardSO>();

    private readonly List<CardSO> _activeCards = new List<CardSO>();
    private bool _initialized;

    public bool CanDraw => _activeCards.Count > 0 || _deckToUse.Exists(card => card != null);

    private void Awake()
    {
        EnsureInitialized();
    }

    public void Initialize(IReadOnlyList<CardSO> definitions)
    {
        _deckToUse.Clear();

        if (definitions != null)
        {
            foreach (CardSO card in definitions)
            {
                if (card != null)
                    _deckToUse.Add(card);
            }
        }

        _initialized = true;
        RefillAndShuffle();
    }

    // Also initialize on demand, so callers don't depend on Awake order.
    private void EnsureInitialized()
    {
        if (_initialized) return;
        _initialized = true;
        RefillAndShuffle();
    }

    private void RefillAndShuffle()
    {
        _activeCards.Clear();
        foreach (CardSO card in _deckToUse)
        {
            if (card != null)
                _activeCards.Add(card);
        }

        for (int i = _activeCards.Count - 1; i > 0; i--)
        {
            int selected = Random.Range(0, i + 1);
            CardSO card = _activeCards[i];
            _activeCards[i] = _activeCards[selected];
            _activeCards[selected] = card;
        }
    }

    public bool TryDraw(out CardSO definition)
    {
        EnsureInitialized();

        // Preserve the existing game's refill-on-empty rule.
        if (_activeCards.Count == 0)
            RefillAndShuffle();

        if (_activeCards.Count == 0)
        {
            definition = null;
            return false;
        }

        int lastIndex = _activeCards.Count - 1;
        definition = _activeCards[lastIndex];
        _activeCards.RemoveAt(lastIndex);
        return true;
    }
}
