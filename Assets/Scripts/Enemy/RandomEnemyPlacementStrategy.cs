using System.Collections.Generic;
using UnityEngine;

// Only decides where to play; never reserves a slot or spends resources.
public sealed class RandomEnemyPlacementStrategy : IEnemyPlacementStrategy
{
    public CardPlacePoint ChoosePlacement(
        IReadOnlyList<CardPlacePoint> enemyPoints,
        IReadOnlyList<CardPlacePoint> playerPoints)
    {
        if (enemyPoints == null) return null;

        var emptyPoints = new List<CardPlacePoint>();
        foreach (CardPlacePoint point in enemyPoints)
        {
            if (point != null && !point.IsPlayerPoint && point.ActiveCard == null)
                emptyPoints.Add(point);
        }

        return emptyPoints.Count == 0 ? null : emptyPoints[Random.Range(0, emptyPoints.Count)];
    }
}
