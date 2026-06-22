using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MainMenuScreen : Screen
{
  [Header("Fade Settings")]
  [Tooltip("Time in seconds to wait before starting the fade-in of the main menu.")]
  [SerializeField] private float _fadeInDuration;

  [Tooltip("Time in seconds to wait before starting the fade-out of the main menu.")]
  [SerializeField] private float _fadeOutDuration;

  [Header("Background Fade Settings")]
  [SerializeField] private CanvasGroup _backgroundCanvasGroup;

  public override IEnumerator OnLoad()
  {
    AsyncOperation operation = ApiClient.Instance.GetUserByIdAsync("1");
    yield return operation;

    Sequence inTransition = DOTween.Sequence();
    inTransition.Join(AnimateLoad());

    yield return AnimateLoad().WaitForCompletion();
  }

  public override IEnumerator OnUnload()
  {
    Sequence outTransition = DOTween.Sequence();
    outTransition.Join(AnimateUnload());

    yield return outTransition.WaitForCompletion();
  }

  protected override Tween AnimateLoad()
  {
    _backgroundCanvasGroup.alpha = 1f;
    AudioManager.Instance.PlayMusic(AudioMusicEnum.MainMenuMusic);
    PanelManager.Instance.OpenPanel(PanelName.MainMenuPanel);

    return DOTween.Sequence()
      .Join(_canvasGroup.DOFade(1f, _fadeInDuration))
      .Join(DOVirtual.Float(0f, 1f, _fadeInDuration, v => AudioManager.Instance.SetMusicVolume(v)));
  }

  protected override Tween AnimateUnload()
  {
    PanelManager.Instance.ClosePanel(PanelName.MainMenuPanel);

    return DOTween.Sequence()
      .Join(_backgroundCanvasGroup.DOFade(0f, _fadeOutDuration))
      .Join(DOVirtual.Float(1f, 0f, _fadeOutDuration, v => AudioManager.Instance.SetMusicVolume(v)));
  }
}
