using System.Collections.Generic;
using UnityEngine;

public sealed class AttackingEnemyPlacementStrategy : IEnemyPlacementStrategy
{
    public CardPlacePoint ChoosePlacement(
        IReadOnlyList<CardPlacePoint> enemyPoints,
        IReadOnlyList<CardPlacePoint> playerPoints)
    {
        if (enemyPoints == null || playerPoints == null ||
            enemyPoints.Count != playerPoints.Count)
        {
            return null;
        }

        var preferredPoints = new List<CardPlacePoint>();
        var secondaryPoints = new List<CardPlacePoint>();

        for (int i = 0; i < enemyPoints.Count; i++)
        {
            CardPlacePoint enemyPoint = enemyPoints[i];
            CardPlacePoint playerPoint = playerPoints[i];

            // We can only place into an empty enemy slot.
            if (enemyPoint == null || playerPoint == null ||
                enemyPoint.IsPlayerPoint || enemyPoint.ActiveCard != null)
            {
                continue;
            }

            if (playerPoint.ActiveCard == null)
                preferredPoints.Add(enemyPoint);
            else
                secondaryPoints.Add(enemyPoint);
        }

        List<CardPlacePoint> candidates =
            preferredPoints.Count > 0
                ? preferredPoints
                : secondaryPoints;

        return candidates.Count == 0
            ? null
            : candidates[Random.Range(0, candidates.Count)];
    }
}