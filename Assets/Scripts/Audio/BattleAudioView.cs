using UnityEngine;

[DisallowMultipleComponent]
public class BattleAudioView : MonoBehaviour
{
    [SerializeField] private BattleController _battle;
    [SerializeField] private AudioPlayer _audioPlayer;

    [Header("Turn Cues")]
    [SerializeField] private AudioCueSO _playerTurnCue;
    [SerializeField] private AudioCueSO _enemyTurnCue;

    [Header("Result Cues")]
    [SerializeField] private AudioCueSO _victoryCue;
    [SerializeField] private AudioCueSO _defeatCue;

    private void Awake()
    {
        if (_audioPlayer == null)
            _audioPlayer = GetComponent<AudioPlayer>();

        if (_battle == null || _audioPlayer == null)
        {
            Debug.LogError("BattleAudioView needs a BattleController and an AudioPlayer.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_battle == null || _audioPlayer == null)
            return;

        _battle.OnPhaseChanged += HandlePhaseChanged;
        _battle.OnBattleEnded += HandleBattleEnded;
    }

    private void Start()
    {
        if (_battle != null && !_battle.IsBattleOver)
            HandlePhaseChanged(_battle.CurrentPhase);
    }

    private void OnDisable()
    {
        if (_battle == null)
            return;

        _battle.OnPhaseChanged -= HandlePhaseChanged;
        _battle.OnBattleEnded -= HandleBattleEnded;
    }

    private void HandlePhaseChanged(TurnOrder phase)
    {
        switch (phase)
        {
            case TurnOrder.playerActive:
                _audioPlayer.PlayOneShot(_playerTurnCue);
                break;
            case TurnOrder.enemyActive:
                _audioPlayer.PlayOneShot(_enemyTurnCue);
                break;
        }
    }

    private void HandleBattleEnded(BattleResult result)
    {
        AudioCueSO cue = result == BattleResult.Victory
            ? _victoryCue
            : _defeatCue;

        _audioPlayer.PlayOneShot(cue);
    }
}
