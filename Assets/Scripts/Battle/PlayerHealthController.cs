using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int _playerHealth;

    public event Action OnPlayerDied;

    public void DamagePlayer(int damage)
    {
        if(_playerHealth > 0)
        {
            _playerHealth -= damage;

            if(_playerHealth <= 0)
            {
                _playerHealth = 0;

                OnPlayerDied?.Invoke();
            }
        }
    }
}
