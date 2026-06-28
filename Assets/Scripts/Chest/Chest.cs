using System;
using UnityEngine;

public class Chest : MonoBehaviour
{
  private bool _isOpen = false;
  [SerializeField] private Animator _animator;

  public Action OnChestOpened;

  private void Awake()
  {
    _animator = GetComponent<Animator>();
  }

  public void OpenChest()
  {
    if (!_isOpen)
    {
      _isOpen = true;
      Debug.Log("Chest is now open!");
    }

    _animator.SetTrigger("chest_open");
    OnChestOpened?.Invoke();
  }

  public void Reset()
  {
    _isOpen = false;
  }
}