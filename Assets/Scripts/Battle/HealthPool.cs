using System;
using UnityEngine;

public class HealthPool : MonoBehaviour
{
    [SerializeField, Min(0)] private int _currentHealth = 50;

    public int CurrentHealth => _currentHealth;
    public bool IsDead => _currentHealth <= 0;

    public event Action<int> OnHealthChanged, OnDamaged;
    public event Action OnDied;

    public void TakeDamage(int damage)
    {
        if (IsDead || damage <= 0) return;

        int damageTaken = Mathf.Min(damage, _currentHealth);
        _currentHealth -= damageTaken;

        OnHealthChanged?.Invoke(_currentHealth);
        OnDamaged?.Invoke(damageTaken);

        if (IsDead)
            OnDied?.Invoke();
    }
}
