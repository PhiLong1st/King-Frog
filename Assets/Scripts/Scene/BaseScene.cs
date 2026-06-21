using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public abstract class BaseScene : MonoBehaviour
{
  private Canvas _canvas;

  public bool IsReady { get; protected set; }

  private void Awake()
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

  public abstract IEnumerator AnimateLoad();

  public abstract IEnumerator AnimateUnload();

  
}