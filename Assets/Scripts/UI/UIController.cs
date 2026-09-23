using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ManaPool _playerMana;
    [SerializeField] private ManaPool _enemyMana;

    [Header("Texts References")]
    [SerializeField] private TextMeshProUGUI _playerManaText;
    [SerializeField] private TextMeshProUGUI _enemyManaText;
    [SerializeField] private TextMeshProUGUI _notEnoughManaText;

    [Header("Timer")]
    [SerializeField] private float _manaWarningTime;

    
    private float _manaWarningCounter;


    private void Start()
    {
        _notEnoughManaText.gameObject.SetActive(false);
        UpdatePlayerMana(_playerMana.Current, _playerMana.Maximum);
        UpdateEnemyMana(_enemyMana.Current, _enemyMana.Maximum);
    }
    private void Update()
    {
        if (_manaWarningCounter > 0)
        {
            _manaWarningCounter -= Time.deltaTime;

            if(_manaWarningCounter <= 0)
            {
                _notEnoughManaText.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        _playerMana.OnManaChanged += UpdatePlayerMana;
        _enemyMana.OnManaChanged += UpdateEnemyMana;
        _playerMana.OnNotEnoughMana += ShowManaWarning;
    }
    private void OnDisable()
    {
        _playerMana.OnManaChanged -= UpdatePlayerMana;
        _enemyMana.OnManaChanged -= UpdateEnemyMana;
        _playerMana.OnNotEnoughMana -= ShowManaWarning;
    }

    private void UpdatePlayerMana(int newMana, int maxMana)
    {
        _playerManaText.text = "Mana: " + newMana + " / " + maxMana;
    }
    private void UpdateEnemyMana(int newMana, int maxMana)
    {
        _enemyManaText.text = "Mana: " + newMana + " / " + maxMana;
    }
    private void ShowManaWarning()
    {
        _notEnoughManaText.gameObject.SetActive(true);
        _manaWarningCounter = _manaWarningTime;
    }
}
