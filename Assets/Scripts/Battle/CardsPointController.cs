using System;
using System.Collections;
using UnityEngine;

public class CardsPointController : MonoBehaviour
{
    [SerializeField] private CardPlacePoint[] _playerPlacements;
    [SerializeField] private CardPlacePoint[] _enemyPlacements;

    private float _timeBetweenAttacks = 0.25f;

    public event Action OnCompletedAttack;

    public IEnumerator RunPlayerAttacks()
    {
        yield return new WaitForSeconds(_timeBetweenAttacks);

        for(int i = 0; i < _playerPlacements.Length; i++)
        {
            if (_playerPlacements[i].ActiveCard != null)
            {
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
}
