using UnityEngine;

public class DamageIndicatorSpawner : MonoBehaviour
{
    [SerializeField] private HealthPool _health;
    [SerializeField] private UIDamageIndicator _indicatorPrefab;
    [SerializeField] private RectTransform _spawnPoint;

    private void OnEnable()
    {
        _health.OnDamaged += ShowDamage;
    }

    private void OnDisable()
    {
        _health.OnDamaged -= ShowDamage;
    }

    private void ShowDamage(int damage)
    {
        UIDamageIndicator indicator =
            Instantiate(_indicatorPrefab, _spawnPoint, false);

        RectTransform rect = indicator.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;

        indicator.Initialize(damage);
    }
}