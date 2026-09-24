using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/Audio Cue")]
public class AudioCueSO : ScriptableObject
{
    [SerializeField] private AudioClip[] _clips = System.Array.Empty<AudioClip>();
    [SerializeField] private AudioMixerGroup _outputMixerGroup;
    [SerializeField] private Vector2 _volumeRange = Vector2.one;
    [SerializeField] private Vector2 _pitchRange = Vector2.one;

    public AudioMixerGroup OutputMixerGroup => _outputMixerGroup;

    public bool TryGetPlaybackSettings(
        out AudioClip clip,
        out float volume,
        out float pitch)
    {
        clip = GetRandomClip();
        volume = Random.Range(_volumeRange.x, _volumeRange.y);
        pitch = Random.Range(_pitchRange.x, _pitchRange.y);

        return clip != null;
    }

    private AudioClip GetRandomClip()
    {
        if (_clips == null || _clips.Length == 0)
            return null;

        return _clips[Random.Range(0, _clips.Length)];
    }

    private void OnValidate()
    {
        _volumeRange.x = Mathf.Clamp01(_volumeRange.x);
        _volumeRange.y = Mathf.Clamp01(_volumeRange.y);
        SortRange(ref _volumeRange);

        _pitchRange.x = Mathf.Clamp(_pitchRange.x, -3f, 3f);
        _pitchRange.y = Mathf.Clamp(_pitchRange.y, -3f, 3f);
        SortRange(ref _pitchRange);
    }

    private static void SortRange(ref Vector2 range)
    {
        if (range.x <= range.y)
            return;

        float previousMinimum = range.x;
        range.x = range.y;
        range.y = previousMinimum;
    }
}
