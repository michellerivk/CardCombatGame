using System.Collections.Generic;

public interface IEnemyPlacementStrategy
{
    CardPlacePoint ChoosePlacement(
        IReadOnlyList<CardPlacePoint> enemyPoints,
        IReadOnlyList<CardPlacePoint> playerPoints);
}
