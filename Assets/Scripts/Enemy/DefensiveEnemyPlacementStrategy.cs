using System.Collections.Generic;
using UnityEngine;

// Prefer blocking an opposing card; otherwise use any remaining empty enemy slot.
public sealed class DefensiveEnemyPlacementStrategy : IEnemyPlacementStrategy
{
    public CardPlacePoint ChoosePlacement(
        IReadOnlyList<CardPlacePoint> enemyPoints,
        IReadOnlyList<CardPlacePoint> playerPoints)
    {
        if (enemyPoints == null || playerPoints == null || enemyPoints.Count != playerPoints.Count)
            return null;

        var preferredPoints = new List<CardPlacePoint>();
        var secondaryPoints = new List<CardPlacePoint>();

        // Rebuild from current occupancy on every decision: no stale reservations.
        for (int i = 0; i < enemyPoints.Count; i++)
        {
            CardPlacePoint enemyPoint = enemyPoints[i];
            CardPlacePoint playerPoint = playerPoints[i];
            if (enemyPoint == null || playerPoint == null || enemyPoint.IsPlayerPoint ||
                enemyPoint.ActiveCard != null)
            {
                continue;
            }

            Card opponent = playerPoint.ActiveCard;
            if (opponent != null && !opponent.IsDefeated)
                preferredPoints.Add(enemyPoint);
            else
                secondaryPoints.Add(enemyPoint);
        }

        List<CardPlacePoint> candidates = preferredPoints.Count > 0 ? preferredPoints : secondaryPoints;
        return candidates.Count == 0 ? null : candidates[Random.Range(0, candidates.Count)];
    }
}
