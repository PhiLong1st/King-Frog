using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public abstract class Panel : MonoBehaviour
{
  public Action OnPanelShow;
  public Action OnPanelHide;

  protected RectTransform rectTransform;
  protected Vector2 _originalPosition;


  protected virtual void Awake()
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