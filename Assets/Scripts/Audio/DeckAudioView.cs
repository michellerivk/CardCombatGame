using UnityEngine;

[DisallowMultipleComponent]
public class DeckAudioView : MonoBehaviour
{
    [SerializeField] private DeckController _deck;
    [SerializeField] private AudioPlayer _audioPlayer;
    [SerializeField] private AudioCueSO _drawCue;

    private void Awake()
    {
        if (_audioPlayer == null)
            _audioPlayer = GetComponent<AudioPlayer>();

        if (_deck == null || _audioPlayer == null)
        {
            Debug.LogError("DeckAudioView needs a DeckController and an AudioPlayer.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_deck != null && _audioPlayer != null)
            _deck.OnCardDrawn += PlayDraw;
    }

    private void OnDisable()
    {
        if (_deck != null)
            _deck.OnCardDrawn -= PlayDraw;
    }

    private void PlayDraw(Card card) =>
        _audioPlayer.PlayOneShot(_drawCue);
}
