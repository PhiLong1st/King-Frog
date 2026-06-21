using UnityEngine;
using DG.Tweening;

public class MainMenuScreen : BaseScreen
{
  [Tooltip("Time in seconds to wait before starting the fade-in of the main menu.")]
  [SerializeField] private float _fadeInDuration = 1f;

  [Tooltip("Time in seconds to wait before starting the fade-out of the main menu.")]
  [SerializeField] private float _fadeOutDuration = 1f;

  [Tooltip("CanvasGroup for the main menu screen, used to control its visibility and interactivity.")]
  [SerializeField] private CanvasGroup _canvasGroup;

  private void Start()
  {
    _canvasGroup.alpha = 0f;
  }

  

  public override Tween AnimateLoad()
  {
    AudioManager.Instance.PlayMusic(AudioMusicEnum.MainMenuMusic);

    return DOTween.Sequence()
      .Join(_canvasGroup.DOFade(1f, _fadeInDuration))
      .Join(DOVirtual.Float(0f, 1f, _fadeInDuration, v => AudioManager.Instance.SetMusicVolume(v)));
  }

  public override Tween AnimateUnload()
  {
    return DOTween.Sequence()
      .Join(_canvasGroup.DOFade(0f, _fadeOutDuration))
      .Join(DOVirtual.Float(1f, 0f, _fadeOutDuration, v => AudioManager.Instance.SetMusicVolume(v)));
  }
}
