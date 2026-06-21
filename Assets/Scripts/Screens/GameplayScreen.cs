using UnityEngine;
using DG.Tweening;
using System.Collections;

public class GameplayScreen : Screen
{
  [Header("Fade Settings")]
  [Tooltip("Time in seconds to wait before starting the fade-in of the main menu.")]
  [SerializeField] private float _fadeInDuration;

  [Tooltip("Time in seconds to wait before starting the fade-out of the main menu.")]
  [SerializeField] private float _fadeOutDuration;

  [SerializeField] private GameObject _level;

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      StartCoroutine(OnUnload());
      _screenManager.OpenScreen(ScreenName.MainMenuScreen);
    }
  }

  public override IEnumerator OnLoad()
  {
    Sequence inTransition = DOTween.Sequence();
    inTransition.Join(AnimateLoad());

    _level.SetActive(true);
    yield return AnimateLoad().WaitForCompletion();
  }

  public override IEnumerator OnUnload()
  {
    Sequence outTransition = DOTween.Sequence();
    outTransition.Join(AnimateUnload());

    yield return outTransition.WaitForCompletion();
    _level.SetActive(false);
  }

  protected override Tween AnimateLoad()
  {
    AudioManager.Instance.PlayMusic(AudioMusicEnum.GameplayMusic);

    return DOTween.Sequence()
      .Append(_canvasGroup.DOFade(1f, _fadeInDuration))
      .Append(_canvasGroup.DOFade(0f, _fadeOutDuration))
      .Join(DOVirtual.Float(0f, 1f, _fadeInDuration, v => AudioManager.Instance.SetMusicVolume(v)));
  }

  protected override Tween AnimateUnload()
  {
    return DOTween.Sequence()
      .Join(_canvasGroup.DOFade(0f, _fadeOutDuration))
      .Join(DOVirtual.Float(1f, 0f, _fadeOutDuration, v => AudioManager.Instance.SetMusicVolume(v)));
  }
}
