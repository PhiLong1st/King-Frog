using UnityEngine;

public class LevelPanel : Panel
{
  [SerializeField] private float _animationDuration = 0.5f;

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