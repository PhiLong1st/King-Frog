using System;
using UnityEngine;

public class Key : MonoBehaviour
{
  public Action OnKeyCollected;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag(TagConstant.Player))
    {
      OnKeyCollected?.Invoke();
      gameObject.SetActive(false);
    }
  }

  public void Reset()
  {
    gameObject.SetActive(true);
  }
}