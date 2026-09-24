using UnityEngine;

[DisallowMultipleComponent]
public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioPlayer _audioPlayer;
    [SerializeField] private AudioCueSO _startingMusic;
    [SerializeField] private bool _playOnStart = true;

    private AudioCueSO _currentMusic;

    private void Awake()
    {
        if (_audioPlayer == null)
            _audioPlayer = GetComponent<AudioPlayer>();

        if (_audioPlayer == null)
        {
            Debug.LogError("MusicController needs an AudioPlayer.", this);
            enabled = false;
        }
    }

    private void Start()
    {
        if (_playOnStart)
            PlayMusic(_startingMusic);
    }

    public void PlayMusic(AudioCueSO music)
    {
        if (music == null || music == _currentMusic)
            return;

        if (_audioPlayer.PlayLoop(music))
            _currentMusic = music;
    }

    public void StopMusic()
    {
        _audioPlayer.Stop();
        _currentMusic = null;
    }
}
