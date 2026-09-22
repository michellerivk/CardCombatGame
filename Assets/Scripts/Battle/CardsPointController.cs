using System;
using System.Collections;
using UnityEngine;

public class CardsPointController : MonoBehaviour
{
    [SerializeField] private CardPlacePoint[] _playerPlacements;
    [SerializeField] private CardPlacePoint[] _enemyPlacements;
    [SerializeField] private Transform _discardPoint;
    [SerializeField] private PlayerHealthController _playerHealthController;

    private float _timeBetweenAttacks = 0.25f;

    public event Action OnCompletedAttack;

    private void OnEnable()
    {
        SubscribeToDefeats(_playerPlacements);
        SubscribeToDefeats(_enemyPlacements);
    }

    private void OnDisable()
    {
        UnsubscribeFromDefeats(_playerPlacements);
        UnsubscribeFromDefeats(_enemyPlacements);
    }

    private void SubscribeToDefeats(CardPlacePoint[] points)
    {
        foreach (CardPlacePoint point in points)
        {
            if (point == null) continue;
            point.OnCardDefeated -= DiscardDefeatedCard;
            point.OnCardDefeated += DiscardDefeatedCard;
        }
    }

    private void UnsubscribeFromDefeats(CardPlacePoint[] points)
    {
        foreach (CardPlacePoint point in points)
        {
            if (point != null)
                point.OnCardDefeated -= DiscardDefeatedCard;
        }
    }

    private void DiscardDefeatedCard(Card card)
    {
        if (card == null) return;

        // The square has released this card; stop interaction during travel.
        if (card.TryGetComponent(out CardSelectionController selection))
            selection.UnSelectCard();
        if (card.TryGetComponent(out CardPointerInput input))
            input.enabled = false;
        if (card.TryGetComponent(out CardPlacementController placement))
            placement.enabled = false;
        foreach (Collider cardCollider in card.GetComponentsInChildren<Collider>())
            cardCollider.enabled = false;

        if (_discardPoint == null || !card.TryGetComponent(out CardMotion motion) ||
            !motion.isActiveAndEnabled)
        {
            Debug.LogError("Discarding a card needs a Discard Point and an enabled CardMotion.", this);
            Destroy(card.gameObject);
            return;
        }

        if (card.TryGetComponent(out CardAnimator cardAnimator))
        {
            cardAnimator.AnimateJump();
        }

        motion.MoveTo(_discardPoint.position, _discardPoint.rotation, () =>
        {
            if (card != null)
                Destroy(card.gameObject);
        });
    }

    public IEnumerator RunPlayerAttacks()
    {
        yield return new WaitForSeconds(_timeBetweenAttacks);

        for(int i = 0; i < _playerPlacements.Length; i++)
        {
            if (_playerPlacements[i].ActiveCard != null)
            {
                _playerPlacements[i].ActiveCard.NotifyAttack();

                if ( _enemyPlacements[i].ActiveCard != null)
                {
                    //Attack Enemy Card
                    _enemyPlacements[i].ActiveCard.
                        DamageCard(_playerPlacements[i].ActiveCard.AttackPower);
                    
                }
                else
                {
                    //Attack boss directly  
                }

                yield return new WaitForSeconds(_timeBetweenAttacks);
            }
        }

        OnCompletedAttack?.Invoke(); // Battle Controller advance turn
    } 

    public IEnumerator RunEnemyAttacks()
    {
        yield return new WaitForSeconds(_timeBetweenAttacks);

        for(int i = 0; i < _enemyPlacements.Length; i++)
        {
            if (_enemyPlacements[i].ActiveCard != null)
            {
                _enemyPlacements[i].ActiveCard.NotifyAttack();

                if ( _playerPlacements[i].ActiveCard != null)
                {
                    //Attack Player Card
                    _playerPlacements[i].ActiveCard.
                        DamageCard(_enemyPlacements[i].ActiveCard.AttackPower);
                    
                }
                else
                {
                    //Attack Player directly  
                    _playerHealthController.DamagePlayer(_enemyPlacements[i].ActiveCard.AttackPower);
                }

                yield return new WaitForSeconds(_timeBetweenAttacks);
            }
        }

        OnCompletedAttack?.Invoke(); // Battle Controller advance turn
    } 
}
