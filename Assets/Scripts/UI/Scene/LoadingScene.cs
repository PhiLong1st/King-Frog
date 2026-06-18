using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
  [Tooltip("Text component to display the current loading percentage (e.g., '75%').")]
  [SerializeField] private TextMeshProUGUI _percentageText;

  [Tooltip("Text component to display loading status (e.g., 'Loading...', 'Loading Complete!').")]
  [SerializeField] private TextMeshProUGUI _loadingText;

  [Tooltip("Speed at which the slider fills up (0.374 means 37.4% per second).")]
  [SerializeField] private float _loadingSpeed = 0.0374f;

  [Tooltip("Time in seconds before the slider updates to the next progress value.")]
  [SerializeField] private float _timeThreshold = 0.2f;

  [Tooltip("Array of progress values the slider will cycle through.")]
  [SerializeField] private float[] _progressValues = { 0f, 0.25f, 0.34f, 0.5f, 0.66f, 0.79f, 0.92f };

  [SerializeField] private Slider _slider;
  [SerializeField] private Image _overlayBackground;

  [SerializeField] private CanvasGroup _loadingSceneCanvasGroup;
  [SerializeField] private CanvasGroup _mainMenuCanvasGroup;

  [SerializeField] private float fadeDuration;

  private float _timeElapsed = 0f;
  private int _currentIndex = 0;
  private float _targetProgress = 0f;
  private bool _isChangingScene;

  public void Awake()
  {
    _slider ??= GetComponentInChildren<Slider>();
  }

  private void Start()
  {
    _isChangingScene = false;
    _currentIndex = 0;
    _timeElapsed = 0f;
    _slider.value = _progressValues[_currentIndex];
  }

  private void Update()
  {
    bool isLoadingComplete = _slider.value >= 1f;
    if (isLoadingComplete && !_isChangingScene)
    {
      _isChangingScene = true;
      _loadingText.text = "Loading Complete!";
      ChangeToMainMenuScene();
      return;
    }

    if (_targetProgress < 1f)
    {
      Check();
    }

    UpdateSlider();
  }

  private string GetLoadingText()
  {
    string _loadingText = "Loading";
    int dotCount = Mathf.FloorToInt(Time.time * 2) % 4;
    string dotCountString = new string('.', dotCount);
    return _loadingText + dotCountString;
  }

  private void UpdateSlider()
  {
    _slider.value = Mathf.Min(_slider.value + _loadingSpeed * Time.deltaTime, _targetProgress);

    _loadingText.text = GetLoadingText();
    _percentageText.text = $"{Mathf.Round(_slider.value * 1000) / 10f}%";
  }

  private void Check()
  {
    _timeElapsed += Time.deltaTime;

    if (_timeElapsed < _timeThreshold)
    {
      return;
    }

    _currentIndex = Mathf.Min(_currentIndex + 1, _progressValues.Length - 1);
    _targetProgress = IsDataLoadDone() ? 1f : _progressValues[_currentIndex];
    _timeElapsed = 0f;
  }

  private bool IsDataLoadDone()
  {
    // MOCK: Simulate data loading completion after 7 seconds
    Debug.Log("Checking if data load is done at " + Time.time + "...");
    return Time.time > 7f;
  }

  private void ChangeToMainMenuScene()
  {
    StartCoroutine(ShowFadeOutAnimation());
  }

  private IEnumerator ShowFadeOutAnimation()
  {
    float elapsedTime = 0f;
    while (elapsedTime < fadeDuration)
    {
      elapsedTime += Time.deltaTime;
      _loadingSceneCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
      _mainMenuCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
      yield return null;
    }

    _loadingSceneCanvasGroup.alpha = 0f;
    gameObject.SetActive(false);
  }
}