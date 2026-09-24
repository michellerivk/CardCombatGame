using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool _ignoreListenerPause;

    private void Awake()
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        _audioSource.playOnAwake = false;
        _audioSource.ignoreListenerPause = _ignoreListenerPause;
    }

    public bool PlayOneShot(AudioCueSO cue)
    {
        if (!TryPrepare(cue, out AudioClip clip, out float volume))
            return false;

        _audioSource.PlayOneShot(clip, volume);
        return true;
    }

    public bool PlayLoop(AudioCueSO cue)
    {
        if (!TryPrepare(cue, out AudioClip clip, out float volume))
            return false;

        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.loop = true;
        _audioSource.Play();
        return true;
    }

    public void Stop()
    {
        _audioSource.Stop();
        _audioSource.clip = null;
        _audioSource.loop = false;
    }

    private bool TryPrepare(
        AudioCueSO cue,
        out AudioClip clip,
        out float volume)
    {
        clip = null;
        volume = 1f;

        if (cue == null || _audioSource == null ||
            !cue.TryGetPlaybackSettings(out clip, out volume, out float pitch))
        {
            return false;
        }

        _audioSource.pitch = pitch;

        if (cue.OutputMixerGroup != null)
            _audioSource.outputAudioMixerGroup = cue.OutputMixerGroup;

        return true;
    }
}
