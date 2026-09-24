using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class ButtonAudioView : MonoBehaviour,
    IPointerEnterHandler,
    IPointerClickHandler,
    ISelectHandler,
    ISubmitHandler
{
    [SerializeField] private AudioPlayer _audioPlayer;
    [SerializeField] private AudioCueSO _highlightCue;
    [SerializeField] private AudioCueSO _clickCue;

    private void Awake()
    {
        if (_audioPlayer == null)
            _audioPlayer = GetComponentInParent<AudioPlayer>();

        if (_audioPlayer == null)
        {
            Debug.LogError("ButtonAudioView needs an AudioPlayer.", this);
            enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioPlayer.PlayOneShot(_highlightCue);
    }

    public void OnPointerClick(PointerEventData eventData) => PlayClick();

    public void OnSelect(BaseEventData eventData) =>
        _audioPlayer.PlayOneShot(_highlightCue);

    public void OnSubmit(BaseEventData eventData) => PlayClick();

    private void PlayClick() => _audioPlayer.PlayOneShot(_clickCue);
}
