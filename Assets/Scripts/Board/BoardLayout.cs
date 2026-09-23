using System.Collections.Generic;
using UnityEngine;

// One source of truth for opposing lanes. Equal indices face one another.
public class BoardLayout : MonoBehaviour
{
    [SerializeField] private CardPlacePoint[] _playerPlacements;
    [SerializeField] private CardPlacePoint[] _enemyPlacements;

    public IReadOnlyList<CardPlacePoint> PlayerPoints => _playerPlacements;
    public IReadOnlyList<CardPlacePoint> EnemyPoints => _enemyPlacements;

    public bool ValidateSetup()
    {
        if (_playerPlacements == null || _enemyPlacements == null ||
            _playerPlacements.Length != _enemyPlacements.Length)
        {
            Debug.LogError("BoardLayout needs matching player and enemy lane arrays.", this);
            return false;
        }

        for (int i = 0; i < _playerPlacements.Length; i++)
        {
            if (_playerPlacements[i] == null || _enemyPlacements[i] == null)
            {
                Debug.LogError($"BoardLayout lane {i} is missing a slot reference.", this);
                return false;
            }
        }

        return true;
    }
}
