using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class MainMenuPanel : Panel
{
  [SerializeField] private float _animationDuration = 0.5f;
  [SerializeField] private CanvasGroup _canvasGroup;

  protected override void Awake()
  {
    base.Awake();
    _canvasGroup = GetComponent<CanvasGroup>();
  }

  public void OnPlayButtonClicked()
  {
    ScreenManager.Instance.CloseScreen(ScreenName.MainMenuScreen);
    ScreenManager.Instance.OpenScreen(ScreenName.GameplayScreen);
    PanelManager.Instance.ClosePanel(PanelName.MainMenuPanel);
  }

  public void OnSettingsButtonClicked()
  {
    // PanelManager.Instance.ClosePanel(PanelName.MainMenuPanel);
    PanelManager.Instance.OpenPanel(PanelName.SettingPanel);
  }

  public void OnExitButtonClicked()
  {
    PanelManager.Instance.ClosePanel(PanelName.MainMenuPanel);
    ScreenManager.Instance.Exit();
  }

  protected override void OnShow()
  {
    IEnumerator fadeInCoroutine = AnimationUtils.FadeAnimation(0f, 1f, _animationDuration, v =>
    {
      _canvasGroup.alpha = v;
      AudioManager.Instance.SetMusicVolume(v);
    });
    StartCoroutine(fadeInCoroutine);
  }

  protected override void OnHide()
  {
    IEnumerator fadeOutCoroutine = AnimationUtils.FadeAnimation(1f, 0f, _animationDuration, v =>
    {
      _canvasGroup.alpha = v;
      AudioManager.Instance.SetMusicVolume(v);
    });
    StartCoroutine(fadeOutCoroutine);
  }
}