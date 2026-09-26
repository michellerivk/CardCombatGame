using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private HealthPool _health;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Image _healthFill;
    [SerializeField] private string _label = "HEALTH";

    private void Start()
    {
        UpdateHealth(_health.CurrentHealth);
    }

    private void OnEnable()
    {
        _health.OnHealthChanged += UpdateHealth;
        UpdateHealth(_health.CurrentHealth);
    }

    private void OnDisable()
    {
        _health.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(int health)
    {
        _healthText.text = $"{_label}: {health}";
        if (_healthFill != null)
            _healthFill.fillAmount = Mathf.Clamp01((float)health / Mathf.Max(1, _health.MaximumHealth));
    }
}
