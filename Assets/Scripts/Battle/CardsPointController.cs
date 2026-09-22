using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CardsPointController : MonoBehaviour
{
    [SerializeField] private CardPlacePoint[] _playerPlacements;
    [SerializeField] private CardPlacePoint[] _enemyPlacements;
    [SerializeField] private HealthPool _playerHealth;
    [SerializeField] private HealthPool _enemyHealth;
    [SerializeField, Min(0f)] private float _timeBetweenAttacks = 0.25f;

    public event Action OnCompletedAttack;

    public IEnumerator RunPlayerAttacks()
    {
        return RunAttacks(_playerPlacements, _enemyPlacements, _enemyHealth);
    }

    public IEnumerator RunEnemyAttacks()
    {
        return RunAttacks(_enemyPlacements, _playerPlacements, _playerHealth);
    }

    private IEnumerator RunAttacks(
        CardPlacePoint[] attackingPoints, CardPlacePoint[] defendingPoints, HealthPool opposingHealth)
    {
        if (attackingPoints == null || defendingPoints == null ||
            attackingPoints.Length != defendingPoints.Length || opposingHealth == null)
        {
            Debug.LogError("Attacks need matching board slot arrays and the opposing HealthPool.", this);
            yield break;
        }

        yield return new WaitForSeconds(_timeBetweenAttacks);

        for (int i = 0; i < attackingPoints.Length; i++)
        {
            if (attackingPoints[i] == null || defendingPoints[i] == null)
            {
                Debug.LogError($"Attack lane {i} is missing a board slot reference.", this);
                continue;
            }

            Card attacker = attackingPoints[i].ActiveCard;
            if (attacker == null || attacker.IsDefeated) continue;

            attacker.NotifyAttack();

            Card defender = defendingPoints[i].ActiveCard;
            if (defender != null)
                defender.DamageCard(attacker.AttackPower);
            else
                opposingHealth.TakeDamage(attacker.AttackPower);

            yield return new WaitForSeconds(_timeBetweenAttacks);
        }

        OnCompletedAttack?.Invoke();
    }
}
