using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("SO")]
    [SerializeField] private CardSO _cardSO;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _manaText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _loreText;
    [SerializeField] private Image _characterArt;
    [SerializeField] private Image _bgArt;


    private int _currentHealth, _attackPower, _manaCost;

    void Start()
    {
        SetupCardData();
    }

    private void SetupCardData()
    {
        _currentHealth = _cardSO.currentHealth;
        _attackPower = _cardSO.attackPower;
        _manaCost = _cardSO.manaCost;


        // Move the following commands to a new view script and make the interactions event based!

        _healthText.text = _currentHealth.ToString();
        _attackText.text = _attackPower.ToString();
        _manaText.text = _manaCost.ToString();

        _nameText.text = _cardSO.cardName;
        _descriptionText.text = _cardSO.actionDescription;
        _loreText.text = _cardSO.cardLore;

        _bgArt.sprite = _cardSO.bgSprite;
        _characterArt.sprite = _cardSO.characterSprite;
    }


}
