using UnityEngine;

[DisallowMultipleComponent]
public class CardAudioView : MonoBehaviour
{
    [SerializeField] private Card _card;
    [SerializeField] private AudioPlayer _audioPlayer;

    [Header("Cues")]
    [SerializeField] private AudioCueSO _attackCue;
    [SerializeField] private AudioCueSO _damageCue;
    [SerializeField] private AudioCueSO _defeatedCue;

    private void Awake()
    {
        if (_card == null)
            _card = GetComponent<Card>();
        if (_audioPlayer == null)
            _audioPlayer = GetComponent<AudioPlayer>();

        if (_card == null || _audioPlayer == null)
        {
            Debug.LogError("CardAudioView needs a Card and an AudioPlayer.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_card == null || _audioPlayer == null)
            return;

        _card.OnAttack += PlayAttack;
        _card.OnDamage += PlayDamage;
        _card.OnDefeated += PlayDefeated;
    }

    private void OnDisable()
    {
        if (_card == null)
            return;

        _card.OnAttack -= PlayAttack;
        _card.OnDamage -= PlayDamage;
        _card.OnDefeated -= PlayDefeated;
    }

    private void PlayAttack() => _audioPlayer.PlayOneShot(_attackCue);

    private void PlayDamage() => _audioPlayer.PlayOneShot(_damageCue);

    private void PlayDefeated(Card card) =>
        _audioPlayer.PlayOneShot(_defeatedCue);
}
