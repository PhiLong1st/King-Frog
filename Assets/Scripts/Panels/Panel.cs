using UnityEngine;
using System;

[RequireComponent(typeof(Canvas))]
public abstract class Panel : MonoBehaviour
{
  public Action OnPanelShow;
  public Action OnPanelHide;

  protected RectTransform rectTransform;
  protected Vector2 _originalPosition;


  private void Awake()
  {
    rectTransform = GetComponent<RectTransform>();
    _originalPosition = rectTransform.anchoredPosition;
  }

  public void ShowPanel()
  {
    OnShow();
    OnPanelShow?.Invoke();
  }

  public void HidePanel()
  {
    OnHide();
    OnPanelHide?.Invoke();
  }

  protected abstract void OnShow();

  protected abstract void OnHide();
}