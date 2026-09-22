using TMPro;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    [SerializeField] private HealthPool _health;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private string _label = "HEALTH";

    private void Awake()
    {
        UpdateHealth(_health.CurrentHealth);
    }

    private void OnEnable()
    {
        _health.OnHealthChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        _health.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(int health)
    {
        _healthText.text = $"{_label}: {health}";
    }
}
