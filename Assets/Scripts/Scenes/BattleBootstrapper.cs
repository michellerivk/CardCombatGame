using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class BattleBootstrapper : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private BattleSettingsSO _defaultSettings;

    [Header("Runtime Systems")]
    [SerializeField] private BattleController _battleController;
    [SerializeField] private HealthPool _playerHealth;
    [SerializeField] private HealthPool _enemyHealth;
    [SerializeField] private ManaPool _playerMana;
    [SerializeField] private ManaPool _enemyMana;
    [SerializeField] private CardDeck _playerDeck;
    [SerializeField] private CardDeck _enemyDeck;
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyController _enemyController;
    [SerializeField] private BattleThemeView _themeView;

    public BattleSettingsSO CurrentSettings { get; private set; }

    private void Awake()
    {
        CurrentSettings = BattleSelection.Current != null
            ? BattleSelection.Current
            : _defaultSettings;

        if (CurrentSettings == null)
        {
            Debug.LogError(
                "BattleBootstrapper needs selected settings or a Default Settings asset.",
                this);
            return;
        }

        if (!HasRequiredReferences())
            return;

        _playerHealth.Initialize(CurrentSettings.PlayerStartingHealth);
        _enemyHealth.Initialize(CurrentSettings.EnemyStartingHealth);

        _playerMana.Initialize(
            CurrentSettings.PlayerStartingMana,
            CurrentSettings.MaximumMana);
        _enemyMana.Initialize(
            CurrentSettings.EnemyStartingMana,
            CurrentSettings.MaximumMana);

        _playerDeck.Initialize(CurrentSettings.PlayerDeck);
        _enemyDeck.Initialize(CurrentSettings.EnemyDeck);
        _enemyAI.Initialize(CurrentSettings.EnemyAIType);
        _enemyController.Initialize(
            CurrentSettings.EnemyCardsPlayedPerTurn,
            CurrentSettings.EnemyOpeningHandSize);
        _battleController.Initialize(
            CurrentSettings.OpeningHandSize,
            CurrentSettings.CardsDrawnPerTurn);

        if (_themeView != null)
        {
            _themeView.Apply(
                CurrentSettings.DeskMaterial,
                CurrentSettings.Background);
        }
    }

    private bool HasRequiredReferences()
    {
        bool valid = _battleController != null &&
                     _playerHealth != null &&
                     _enemyHealth != null &&
                     _playerMana != null &&
                     _enemyMana != null &&
                     _playerDeck != null &&
                     _enemyDeck != null &&
                     _enemyAI != null &&
                     _enemyController != null;

        if (!valid)
            Debug.LogError("BattleBootstrapper is missing runtime references.", this);

        return valid;
    }
}
