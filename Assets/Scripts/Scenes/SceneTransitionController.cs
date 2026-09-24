using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    public static SceneTransitionController Instance { get; private set; }

    [SerializeField] private CanvasGroup _fadeScreen;
    [SerializeField, Min(0f)] private float _fadeDuration = 0.4f;

    private bool _isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _fadeScreen.alpha = 0f;
        _fadeScreen.blocksRaycasts = false;
    }

    public void LoadScene(string sceneName)
    {
        if (!_isTransitioning)
            StartCoroutine(TransitionToScene(sceneName));
    }

    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        _isTransitioning = true;
        _fadeScreen.blocksRaycasts = true;

        yield return FadeTo(1f);

        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
        yield return loading;

        // Give the new scene one frame to initialize behind the black screen.
        yield return null;

        yield return FadeTo(0f);

        _fadeScreen.blocksRaycasts = false;
        _isTransitioning = false;
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        if (_fadeDuration <= 0f)
        {
            _fadeScreen.alpha = targetAlpha;
            yield break;
        }

        while (!Mathf.Approximately(_fadeScreen.alpha, targetAlpha))
        {
            _fadeScreen.alpha = Mathf.MoveTowards(
                _fadeScreen.alpha,
                targetAlpha,
                Time.unscaledDeltaTime / _fadeDuration);

            yield return null;
        }

        _fadeScreen.alpha = targetAlpha;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}