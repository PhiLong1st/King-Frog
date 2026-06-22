using UnityEngine;

public class SettingPanel : Panel
{
  [SerializeField] private float _animationDuration = 0.5f;

  public float MusicAudioVolume
  {
    get => AudioManager.Instance.GetMusicVolume();
    set => AudioManager.Instance.SetMusicVolume(value);
  }

  public float SFXAudioVolume
  {
    get => AudioManager.Instance.GetSFXVolume();
    set => AudioManager.Instance.SetSFXVolume(value);
  }

  protected override void OnShow()
  {
    Vector2 startPosition = rectTransform.anchoredPosition;
    Vector2 endPosition = new Vector2(0, 0);
    StartCoroutine(AnimationUtils.Slide(rectTransform, startPosition, endPosition, _animationDuration));
  }

  protected override void OnHide()
  {
    Vector2 startPosition = rectTransform.anchoredPosition;
    Vector2 endPosition = _originalPosition;
    StartCoroutine(AnimationUtils.Slide(rectTransform, startPosition, endPosition, _animationDuration));
  }
}