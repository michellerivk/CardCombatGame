using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ManaPool _mana;

    [Header("Texts References")]
    [SerializeField] private TextMeshProUGUI _playerManaText;
    [SerializeField] private TextMeshProUGUI _notEnoughManaText;

    [Header("Timer")]
    [SerializeField] private float _manaWarningTime;

    
    private float _manaWarningCounter;


    private void Start()
    {
        _notEnoughManaText.gameObject.SetActive(false);
        UpdatePlayerMana(_mana.Current, _mana.Maximum);
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
        _mana.OnManaChanged += UpdatePlayerMana;
        _mana.OnNotEnoughMana += ShowManaWarning;
    }
    private void OnDisable()
    {
        _mana.OnManaChanged -= UpdatePlayerMana;
        _mana.OnNotEnoughMana -= ShowManaWarning;
    }

    private void UpdatePlayerMana(int newMana, int maxMana)
    {
        _playerManaText.text = "Mana: " + newMana + " / " + maxMana;
    }
    private void ShowManaWarning()
    {
        _notEnoughManaText.gameObject.SetActive(true);
        _manaWarningCounter = _manaWarningTime;
    }
}
