using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public abstract class Screen : MonoBehaviour
{
  [Header("Screen Settings")]
  [Tooltip("Canvas component for the screen, used to control its rendering order.")]
  private Canvas _canvas;

  [Tooltip("CanvasGroup for the main menu screen, used to control its visibility and interactivity.")]
  [SerializeField] protected CanvasGroup _canvasGroup;

  protected ScreenManager _screenManager;

  protected virtual void Awake()
  {
    _canvas ??= GetComponentInChildren<Canvas>();
    _canvasGroup ??= GetComponentInChildren<CanvasGroup>();
  }

  protected void ChangeRenderOrder(int order)
  {
    if (_canvas == null)
    {
      Debug.LogWarning("Canvas component is not assigned or found in children.");
      return;
    }

    _canvas.sortingOrder = order;
  }

  public void Initialize(ScreenManager screenManager)
  {
    _screenManager = screenManager;
  }

  public abstract IEnumerator OnLoad();

  public abstract IEnumerator OnUnload();

  protected abstract Tween AnimateLoad();

  protected abstract Tween AnimateUnload();
}