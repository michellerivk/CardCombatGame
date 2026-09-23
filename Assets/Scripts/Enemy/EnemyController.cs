using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private CardDeck _deck;
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyCardPlayer _cardPlayer;
    [SerializeField] private BoardLayout _board;
    [SerializeField] private ManaPool _enemyMana;
    [SerializeField, Min(0)] private int _cardsPerTurn = 5;
    [SerializeField, Min(0f)] private float _timeBetweenPlays = 0.5f;

    [SerializeField, Min(0)] private int _startHandSize;
    private List<CardSO> _cardsInHand = new List<CardSO>();
    private CardSO _pendingCard;

    private void Awake()
    {
        if (_enemyAI == null)
            _enemyAI = GetComponent<EnemyAI>();
        if (_enemyMana == null)
            _enemyMana = GetComponent<ManaPool>();
        if (_cardPlayer == null)
            _cardPlayer = GetComponent<EnemyCardPlayer>();
    }

    private void Start()
    {
        if (_enemyAI == null)
        {
            Debug.LogError("EnemyController needs an EnemyAI to choose whether to set up a hand.", this);
            return;
        }

        if (_enemyAI.CurrentType != EnemyAI.AIType.placeFromDeck)
        {
            SetupHand();
        }
    }

    // BattleController waits for this routine before starting enemy attacks.
    public IEnumerator RunTurn()
    {
        if (_enemyMana == null || _enemyAI == null || _deck == null || _cardPlayer == null || _board == null)
        {
            Debug.LogError("EnemyController is missing required references.", this);
            yield break;
        }

        if (!_board.ValidateSetup() || !_cardPlayer.ValidateSetup())
            yield break;

        for (int i = 0; i < _cardsPerTurn; i++)
        {
            yield return new WaitForSeconds(_timeBetweenPlays);

            CardPlacePoint point = _enemyAI.ChoosePlacement(_board);
            // Don't consume a card when there is nowhere to play it.
            if (point == null)
                yield break;

            bool fromDeck = _enemyAI.CurrentType == EnemyAI.AIType.placeFromDeck;
            CardSO definition;
            if (fromDeck)
            {
                if (_pendingCard == null && !_deck.TryDraw(out _pendingCard))
                    yield break;

                definition = _enemyAI.ChoosePlayableCard(
                    new[] { _pendingCard }, _enemyMana.Current);
            }
            else
            {
                definition = _enemyAI.ChoosePlayableCard(_cardsInHand, _enemyMana.Current);
            }

            if (definition == null)
                yield break;

            bool arrived = false;
            if (!_cardPlayer.TryPlay(definition, point, _enemyMana, out Card card, () => arrived = true))
                yield break;

            // Hand/deck ownership stays here; failed plays leave the source untouched.
            if (fromDeck)
                _pendingCard = null;
            else
                _cardsInHand.Remove(definition);

            CardMotion motion = card.GetComponent<CardMotion>();

            // A defeated card may get a new discard movement before it arrives.
            while (!arrived && card != null && !card.IsDefeated &&
                   motion != null && motion.isActiveAndEnabled)
            {
                yield return null;
            }
        }
    }

    // Setting up the enemy hand
    private void SetupHand()
    {
        _cardsInHand.Clear();
        DrawCardsToHand(_startHandSize);
    }

    // BattleController supplies the same draw count used for the player's turn.
    public void DrawCardsToHand(int count)
    {
        if (count <= 0)
            return;

        if (_enemyAI == null)
        {
            Debug.LogError("Drawing enemy cards needs an EnemyAI.", this);
            return;
        }

        // This mode plays directly from the deck and deliberately has no hand.
        if (_enemyAI.CurrentType == EnemyAI.AIType.placeFromDeck)
            return;

        if (_deck == null)
        {
            Debug.LogError("Drawing enemy cards needs a CardDeck.", this);
            return;
        }

        for (int i = 0; i < count; i++)
        {
            if (!_deck.TryDraw(out CardSO definition))
                break;

            _cardsInHand.Add(definition);
        }
    }
}
