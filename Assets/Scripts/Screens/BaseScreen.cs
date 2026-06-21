using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Canvas))]
public abstract class BaseScreen : MonoBehaviour
{
  private Canvas _canvas;

  public bool IsReady { get; protected set; }

  protected virtual void Awake()
  {
    _canvas ??= GetComponentInChildren<Canvas>();
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

  protected void ChangeSceneState(bool isReady)
  {
    IsReady = isReady;
  }

  public abstract Tween AnimateLoad();

  public abstract Tween AnimateUnload();
}