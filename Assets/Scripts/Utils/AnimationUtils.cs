using System;
using System.Collections;
using UnityEngine;

public static class AnimationUtils
{
  public static IEnumerator FadeAnimation(float startAlpha, float targetAlpha, float duration, Action<float> onUpdate)
  {
    float elapsedTime = 0f;
    while (elapsedTime < duration)
    {
      elapsedTime += Time.deltaTime;

      float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
      onUpdate?.Invoke(currentAlpha);

      yield return null;
    }

    onUpdate?.Invoke(targetAlpha);
  }

  public static IEnumerator Slide(RectTransform target, Vector2 startPosition, Vector2 endPosition, float duration)
  {
    float elapsedTime = 0f;
    while (elapsedTime < duration)
    {
      elapsedTime += Time.deltaTime;
      target.anchoredPosition = Vector2.Lerp(startPosition, endPosition, elapsedTime / duration);
      yield return null;
    }
  }
}