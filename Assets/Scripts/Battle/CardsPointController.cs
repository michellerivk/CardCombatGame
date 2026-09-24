using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsPointController : MonoBehaviour
{
    [SerializeField] private BoardLayout _board;
    [SerializeField] private HealthPool _playerHealth;
    [SerializeField] private HealthPool _enemyHealth;
    [SerializeField, Min(0f)] private float _timeBetweenAttacks = 0.25f;

    public event Action OnCompletedAttack;

    public IEnumerator RunPlayerAttacks()
    {
        if (!ValidateBoard()) yield break;
        yield return RunAttacks(_board.PlayerPoints, _board.EnemyPoints, _enemyHealth);
    }

    public IEnumerator RunEnemyAttacks()
    {
        if (!ValidateBoard()) yield break;
        yield return RunAttacks(_board.EnemyPoints, _board.PlayerPoints, _playerHealth);
    }

    private bool ValidateBoard()
    {
        if (_board != null)
            return _board.ValidateSetup();

        Debug.LogError("CardsPointController needs a BoardLayout.", this);
        return false;
    }

    private IEnumerator RunAttacks(
        IReadOnlyList<CardPlacePoint> attackingPoints, IReadOnlyList<CardPlacePoint> defendingPoints,
        HealthPool opposingHealth)
    {
        if (attackingPoints == null || defendingPoints == null ||
            attackingPoints.Count != defendingPoints.Count || opposingHealth == null)
        {
            Debug.LogError("Attacks need matching board slot arrays and the opposing HealthPool.", this);
            yield break;
        }

        yield return new WaitForSeconds(_timeBetweenAttacks);

        for (int i = 0; i < attackingPoints.Count; i++)
        {
            if (attackingPoints[i] == null || defendingPoints[i] == null)
            {
                Debug.LogError($"Attack lane {i} is missing a board slot reference.", this);
                continue;
            }

            Card attacker = attackingPoints[i].ActiveCard;
            if (attacker == null || attacker.IsDefeated) continue;

            Card defender = defendingPoints[i].ActiveCard;
            CombatResolver.ResolveAttack(attacker, defender, opposingHealth);

            yield return new WaitForSeconds(_timeBetweenAttacks);
        }

        OnCompletedAttack?.Invoke();
    }
}
