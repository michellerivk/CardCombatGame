using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject _pauseScreen;

    private bool _isPaused;
    private float _timeScaleBeforePause = 1f;
    private bool _audioWasPaused;

    public bool IsPaused => _isPaused;

    private void Awake()
    {
        if (_pauseScreen == null)
        {
            Debug.LogError("PauseController needs a Pause Screen.", this);
            enabled = false;
            return;
        }

        _pauseScreen.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (_isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (_isPaused)
            return;

        _isPaused = true;
        _timeScaleBeforePause = Time.timeScale;
        _audioWasPaused = AudioListener.pause;

        _pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        if (!_isPaused)
        {
            _pauseScreen.SetActive(false);
            return;
        }

        Time.timeScale = _timeScaleBeforePause;
        AudioListener.pause = _audioWasPaused;
        _isPaused = false;
        _pauseScreen.SetActive(false);
    }

    private void OnDisable()
    {
        RestoreGameState();
    }

    private void RestoreGameState()
    {
        if (!_isPaused)
            return;

        Time.timeScale = _timeScaleBeforePause;
        AudioListener.pause = _audioWasPaused;
        _isPaused = false;

        if (_pauseScreen != null)
            _pauseScreen.SetActive(false);
    }
}
