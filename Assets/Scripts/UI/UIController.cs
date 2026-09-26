using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ManaPool _playerMana;
    [SerializeField] private ManaPool _enemyMana;

    [Header("Mana Pips (left to right)")]
    [SerializeField] private Image[] _playerFullPips;
    [SerializeField] private Image[] _playerEmptyPips;
    [SerializeField] private Image[] _enemyFullPips;
    [SerializeField] private Image[] _enemyEmptyPips;
    [Header("Warning")]
    [SerializeField] private GameObject _notEnoughManaText;

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
        UpdatePlayerMana(_playerMana.Current, _playerMana.Maximum);
        UpdateEnemyMana(_enemyMana.Current, _enemyMana.Maximum);
    }
    private void OnDisable()
    {
        _playerMana.OnManaChanged -= UpdatePlayerMana;
        _enemyMana.OnManaChanged -= UpdateEnemyMana;
        _playerMana.OnNotEnoughMana -= ShowManaWarning;
    }

    private void UpdatePlayerMana(int newMana, int maxMana)
    {
        UpdatePips(_playerFullPips, _playerEmptyPips, newMana, maxMana);
    }
    private void UpdateEnemyMana(int newMana, int maxMana)
    {
        UpdatePips(_enemyFullPips, _enemyEmptyPips, newMana, maxMana);
    }
    private static void UpdatePips(Image[] fullPips, Image[] emptyPips, int current, int maximum)
    {
        if (fullPips != null)
            for (int i = 0; i < fullPips.Length; i++)
                SetPipVisible(fullPips[i], i < current && i < maximum);
        if (emptyPips != null)
            for (int i = 0; i < emptyPips.Length; i++)
                SetPipVisible(emptyPips[i], i < maximum);
    }

    private static void SetPipVisible(Image pip, bool visible)
    {
        if (pip == null) return;

        // Disabled Images stop supplying preferred dimensions to the layout group.
        // Transparency hides the artwork while preserving identical slot sizes.
        pip.enabled = true;
        pip.preserveAspect = true;
        Color color = pip.color;
        color.a = visible ? 1f : 0f;
        pip.color = color;
        pip.raycastTarget = false;
    }
    private void ShowManaWarning()
    {
        _notEnoughManaText.gameObject.SetActive(true);
        _manaWarningCounter = _manaWarningTime;
    }
}
