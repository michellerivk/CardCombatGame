using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _manaText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _loreText;
    [SerializeField] private Image _characterArt;
    [SerializeField] private Image _bgArt;

    [SerializeField] private Card _card;

    void OnEnable()
    {
        _card.OnSetup += SetupCardView;
    }

    public void SetupCardView()
    {
        _healthText.text = _card.CurrentHealth.ToString();
        _attackText.text = _card.AttackPower.ToString();
        _manaText.text = _card.ManaCost.ToString();

        _nameText.text = _card.CardName;
        _descriptionText.text = _card.CardDescription;
        _loreText.text = _card.CardLore;

        _bgArt.sprite = _card.CardBG;
        _characterArt.sprite = _card.CardCharacter;
    }

}
