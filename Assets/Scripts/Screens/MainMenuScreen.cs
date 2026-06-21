using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class MainMenuScreen : Screen
{
  [Header("Fade Settings")]
  [Tooltip("Time in seconds to wait before starting the fade-in of the main menu.")]
  [SerializeField] private float _fadeInDuration;

  [Tooltip("Time in seconds to wait before starting the fade-out of the main menu.")]
  [SerializeField] private float _fadeOutDuration;

  [Header("User Data UI References")]
  [Tooltip("Text component to display the user's name.")]
  [SerializeField] private TextMeshProUGUI _userNameText;

  [Tooltip("Text component to display the user's ID.")]
  [SerializeField] private TextMeshProUGUI _userIDText;

  [Tooltip("Image component to display the user's avatar.")]
  [SerializeField] private Image _userAvatarImage;

  public override IEnumerator OnLoad()
  {
    Tween loadingTween = _screenManager.ShowLoadingScene();
    loadingTween.Play();

    AsyncOperation operation = ApiClient.Instance.GetUserByIdAsync("1");
    yield return operation;

    Sequence inTransition = DOTween.Sequence();
    inTransition.Join(_screenManager.HideLoadingScene());
    inTransition.Join(AnimateLoad());

    yield return inTransition.WaitForCompletion();
  }

  public override IEnumerator OnUnload()
  {
    Sequence outTransition = DOTween.Sequence();
    outTransition.Join(AnimateUnload());

    yield return outTransition.WaitForCompletion();
  }

  protected override Tween AnimateLoad()
  {
    AudioManager.Instance.PlayMusic(AudioMusicEnum.MainMenuMusic);

    return DOTween.Sequence()
      .Join(_canvasGroup.DOFade(1f, _fadeInDuration))
      .Join(DOVirtual.Float(0f, 1f, _fadeInDuration, v => AudioManager.Instance.SetMusicVolume(v)));
  }

  protected override Tween AnimateUnload()
  {
    return DOTween.Sequence()
      .Join(_canvasGroup.DOFade(0f, _fadeOutDuration))
      .Join(DOVirtual.Float(1f, 0f, _fadeOutDuration, v => AudioManager.Instance.SetMusicVolume(v)));
  }
}
