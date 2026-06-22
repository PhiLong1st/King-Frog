using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScene : Singleton<LoadingScene>
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

  [Header("Duration Settings")]
  [Tooltip("Minimum time in seconds the loading scene must be shown before transitioning.")]
  [SerializeField] private float _minDuration = 4f;

  private float _timeElapsed;
  private int _currentIndex;
  private float _targetProgress;
  private readonly string[] _animatedLoadingText = { "Loading", "Loading.", "Loading..", "Loading..." };
  private bool _isLoadingComplete;

  protected override void Awake()
  {
    base.Awake();
    _slider ??= GetComponentInChildren<Slider>();
    InitState();
  }

  private IEnumerator Start()
  {
    yield return StartCoroutine(TransitionToMainMenu());
  }

  private void Update()
  {
    UpdateSlider();
    UpdatePercentageText();
    UpdateLoadingText();
  }

  private IEnumerator TransitionToMainMenu()
  {
    float startTime = Time.time;

    AsyncOperation mainMenuLoad = SceneManager.LoadSceneAsync(SceneConstant.GameScene, LoadSceneMode.Additive);
    mainMenuLoad.allowSceneActivation = false;

    while (Time.time - startTime < _minDuration || mainMenuLoad.progress < 0.9f)
    {
      yield return null;
    }

    _isLoadingComplete = true;
    mainMenuLoad.allowSceneActivation = true;

    yield return _loadingSceneCanvasGroup.DOFade(0f, _fadeOutDuration).WaitForCompletion();
    SceneManager.UnloadSceneAsync(SceneConstant.LoadingScene);
  }

  private void InitState()
  {
    _currentIndex = 0;
    _targetProgress = _progressValues[_currentIndex];
    _isLoadingComplete = false;
    _timeElapsed = 0f;
    _slider.value = 0f;
    _loadingText.text = "Loading";
    _percentageText.text = "0.0%";
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
      return;

    _timeElapsed -= _timeThreshold;
    _currentIndex = Mathf.Min(_currentIndex + 1, _progressValues.Length - 1);
    _targetProgress = Mathf.Max(_targetProgress, _progressValues[_currentIndex]);

    _slider.value = _isLoadingComplete ? 1f : Mathf.Min(_slider.value + (_loadingSpeed * Time.deltaTime), _targetProgress);
  }
}
