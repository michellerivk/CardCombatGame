using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField] private ManaPool _playerMana;
    [SerializeField] private ManaPool _enemyMana;

    private void StartPlayerTurn()
    {
        //_playerMana.RefillForNewTurn();
    }
}
