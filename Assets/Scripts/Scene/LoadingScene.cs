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

  [Tooltip("CanvasGroup for the loading scene, used to control its visibility and interactivity.")]
  [SerializeField] private CanvasGroup _loadingSceneCanvasGroup;

  [Header("Simulation Settings")]
  [Tooltip("Speed at which the slider fills up (0.0374 means 3.74% per second).")]
  [SerializeField] private float _loadingSpeed;

  [Tooltip("Time in seconds before the slider updates to the next progress value.")]
  [SerializeField] private float _timeThreshold;

  [Tooltip("Array of progress values the slider will cycle through.")]
  [SerializeField] private float[] _progressValues;

  [Header("Fade Settings")]
  [Tooltip("Time in seconds for the fade-out.")]
  [SerializeField] private float _fadeOutDuration;

  [Tooltip("Time in seconds for the fade-in.")]
  [SerializeField] private float _fadeInDuration;

  [Tooltip("Time in seconds to wait before starting the fade-out.")]
  [SerializeField] private float _waitBeforeFadeOutDuration;

  private float _timeElapsed;
  private int _currentIndex;
  private float _targetProgress;
  private readonly string[] _animatedLoadingText = { "Loading", "Loading.", "Loading..", "Loading..." };
  private bool _isLoadingComplete;

  protected override void Awake()
  {
    base.Awake();
    _slider ??= GetComponentInChildren<Slider>();
  }

  private void Update()
  {
    // if (Input.GetKeyDown(KeyCode.Space))
    // {
    //   StartCoroutine(AnimateLoad());
    // }

    // if (Input.GetKeyDown(KeyCode.Escape))
    // {
    //   StartCoroutine(AnimateUnload());
    // }

    UpdateSlider();
    UpdatePercentageText();
    UpdateLoadingText();
  }

  public override IEnumerator AnimateLoad()
  {
    ResetState();

    IEnumerator fadeInCoroutine = AnimationUtils.FadeAnimation(0f, 1f, _fadeInDuration, alpha =>
    {
      _loadingSceneCanvasGroup.alpha = alpha;
    });

    yield return StartCoroutine(fadeInCoroutine);
  }

  public override IEnumerator AnimateUnload()
  {
    _isLoadingComplete = true;

    WaitForSeconds waitBeforeFadeOut = new WaitForSeconds(_waitBeforeFadeOutDuration);
    yield return waitBeforeFadeOut;

    IEnumerator fadeOutCoroutine = AnimationUtils.FadeAnimation(1f, 0f, _fadeOutDuration, alpha =>
    {
      _loadingSceneCanvasGroup.alpha = alpha;
    });

    yield return StartCoroutine(fadeOutCoroutine);
  }

  private void ResetState()
  {
    _currentIndex = 0;
    _targetProgress = _progressValues[_currentIndex];
    _isLoadingComplete = false;
    _timeElapsed = 0f;
    _slider.value = 0f;
    _loadingText.text = "Loading";
    _percentageText.text = "0.0%";
    _loadingSceneCanvasGroup.alpha = 0f;
  }

  private void UpdatePercentageText()
  {
    float percentage = _slider.value * 100f;
    _percentageText.text = $"{percentage:F1}%";
  }

  private void UpdateLoadingText()
  {
    int dotIndex = Mathf.FloorToInt(Time.time * 2f) % _animatedLoadingText.Length;
    _loadingText.text = _isLoadingComplete ? "Loading Complete!" : _animatedLoadingText[dotIndex];
  }

  private void UpdateSlider()
  {
    _timeElapsed += Time.deltaTime;

    if (_timeElapsed < _timeThreshold)
    {
      return;
    }

    _timeElapsed -= _timeThreshold;
    _currentIndex = Mathf.Min(_currentIndex + 1, _progressValues.Length - 1);
    _targetProgress = Mathf.Max(_targetProgress, _progressValues[_currentIndex]);

    _slider.value = _isLoadingComplete ? 1f : Mathf.Min(_slider.value + (_loadingSpeed * Time.deltaTime), _targetProgress);
  }
}