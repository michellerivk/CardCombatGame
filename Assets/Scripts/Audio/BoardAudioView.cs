using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BoardAudioView : MonoBehaviour
{
    [SerializeField] private BoardLayout _board;
    [SerializeField] private AudioPlayer _audioPlayer;
    [SerializeField] private AudioCueSO _placedCue;

    private readonly HashSet<CardPlacePoint> _subscribedPoints = new();

    private void Awake()
    {
        if (_audioPlayer == null)
            _audioPlayer = GetComponent<AudioPlayer>();

        if (_board == null || _audioPlayer == null)
        {
            Debug.LogError("BoardAudioView needs a BoardLayout and an AudioPlayer.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_board == null || _audioPlayer == null)
            return;

        SubscribeTo(_board.PlayerPoints);
        SubscribeTo(_board.EnemyPoints);
    }

    private void OnDisable()
    {
        foreach (CardPlacePoint point in _subscribedPoints)
        {
            if (point != null)
                point.OnCardAssigned -= PlayPlaced;
        }

        _subscribedPoints.Clear();
    }

    private void SubscribeTo(IReadOnlyList<CardPlacePoint> points)
    {
        foreach (CardPlacePoint point in points)
        {
            if (point == null || !_subscribedPoints.Add(point))
                continue;

            point.OnCardAssigned += PlayPlaced;
        }
    }

    private void PlayPlaced(Card card) =>
        _audioPlayer.PlayOneShot(_placedCue);
}
