using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingScene : BaseScene
{
  [Header("UI References")]
  [Tooltip("Slider component representing the loading progress.")]
  [SerializeField] private Slider _slider;

  [Tooltip("Text component to display the current loading percentage (e.g., '75.0%').")]
  [SerializeField] private TextMeshProUGUI _percentageText;

  [Tooltip("Text component to display loading status (e.g., 'Loading...', 'Loading Complete!').")]
  [SerializeField] private TextMeshProUGUI _loadingText;

  [Tooltip("Background image for the loading overlay.")]
  [SerializeField] private Image _overlayBackground;

  [Tooltip("CanvasGroup for the loading scene, used to control its visibility and interactivity.")]
  [SerializeField] private CanvasGroup _loadingSceneCanvasGroup;

  [Header("Simulation Settings")]
  [Tooltip("Speed at which the slider fills up (0.0374 means 3.74% per second).")]
  [SerializeField] private float _loadingSpeed = 0.0374f;

  [Tooltip("Time in seconds before the slider updates to the next progress value.")]
  [SerializeField] private float _timeThreshold = 0.2f;

  [Tooltip("Array of progress values the slider will cycle through.")]
  [SerializeField] private float[] _progressValues = { 0f, 0.25f, 0.34f, 0.5f, 0.66f, 0.79f, 0.92f };

  [Header("Fade Settings")]
  [Tooltip("Time in seconds to wait before starting the fade-out.")]
  [SerializeField] private float _fadeOutDuration = 1f;

  [Tooltip("Time in seconds to wait before starting the fade-in.")]
  [SerializeField] private float _fadeInDuration = 1f;

  private float _timeElapsed = 0f;
  private int _currentIndex = 0;
  private float _targetProgress = 0f;
  private readonly string[] _animatedLoadingText = { "Loading", "Loading.", "Loading..", "Loading..." };

  private void Awake()
  {
    _slider ??= GetComponentInChildren<Slider>();
  }

  private void InitializeLoadingScene()
  {
    _currentIndex = 0;
    _targetProgress = _progressValues[_currentIndex];

    _timeElapsed = 0f;

    _slider.value = 0f;
    _loadingText.text = "Loading";
    _percentageText.text = "0.0%";
    _loadingSceneCanvasGroup.alpha = 0f;
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      StartCoroutine(AnimateLoad());
    }

    if (Input.GetKeyDown(KeyCode.Escape))
    {
      StartCoroutine(AnimateUnload());
    }

    SimulateProgress();
    UpdateUI();
  }

  public override IEnumerator AnimateLoad()
  {
    Debug.Log("LoadingScene: AnimateLoad called.");
    InitializeLoadingScene();
    yield return FadeAnimation(0f, 1f, _fadeInDuration);
  }

  public override IEnumerator AnimateUnload()
  {
    _slider.value = 1f;
    _targetProgress = 1f;
    _loadingText.text = "Loading Complete!";
    Debug.Log("LoadingScene: AnimateUnload called.");

    yield return FadeAnimation(1f, 0f, _fadeOutDuration);
  }

  private void SimulateProgress()
  {
    _timeElapsed += Time.deltaTime;

    if (_timeElapsed < _timeThreshold)
    {
      return;
    }

    _timeElapsed -= _timeThreshold;
    _currentIndex = Mathf.Min(_currentIndex + 1, _progressValues.Length - 1);
    _targetProgress = Mathf.Max(_targetProgress, _progressValues[_currentIndex]);
  }

  private void UpdateUI()
  {
    _slider.value = Mathf.Min(_slider.value + _loadingSpeed * Time.deltaTime, _targetProgress);

    float percentage = _slider.value * 100f;
    _percentageText.text = $"{percentage:F1}%";

    int dotIndex = Mathf.FloorToInt(Time.time * 2f) % 4;
    _loadingText.text = _animatedLoadingText[dotIndex];
  }

  private IEnumerator FadeAnimation(float startAlpha, float targetAlpha, float duration)
  {
    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
      elapsedTime += Time.deltaTime;
      _loadingSceneCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
      yield return null;
    }

    _loadingSceneCanvasGroup.alpha = targetAlpha;
  }
}