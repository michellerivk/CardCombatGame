using UnityEngine;

[DisallowMultipleComponent]
public class HealthAudioView : MonoBehaviour
{
    [SerializeField] private HealthPool _health;
    [SerializeField] private AudioPlayer _audioPlayer;
    [SerializeField] private AudioCueSO _damagedCue;

    private void Awake()
    {
        if (_audioPlayer == null)
            _audioPlayer = GetComponent<AudioPlayer>();

        if (_health == null || _audioPlayer == null)
        {
            Debug.LogError("HealthAudioView needs a HealthPool and an AudioPlayer.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_health != null && _audioPlayer != null)
            _health.OnDamaged += PlayDamaged;
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.OnDamaged -= PlayDamaged;
    }

    private void PlayDamaged(int damage) =>
        _audioPlayer.PlayOneShot(_damagedCue);
}
