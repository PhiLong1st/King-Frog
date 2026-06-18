using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
  [SerializeField] private bool _shouldPause;

  public void Show()
  {
    if (_shouldPause)
    {
      Time.timeScale = 0f;
    }

    gameObject.SetActive(true);
  }

  public void Hide()
  {
    if (_shouldPause)
    {
      Time.timeScale = 1f;
    }

    gameObject.SetActive(false);
  }
}