using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
  [Header("Input Actions")]
  [SerializeField] private InputActionReference _jumpActionRef;
  [SerializeField] private InputActionReference _moveActionRef;

  public bool IsJumpPressed { get; private set; }
  public bool IsMoveLeftPressed { get; private set; }
  public bool IsMoveRightPressed { get; private set; }

  private void OnEnable()
  {
    _jumpActionRef.action.Enable();
    _moveActionRef.action.Enable();
  }

  private void Update()
  {
    IsJumpPressed = _jumpActionRef.action.IsPressed();

    var moveInputValue = _moveActionRef.action.ReadValue<Vector2>();
    IsMoveLeftPressed = moveInputValue.x < 0f;
    IsMoveRightPressed = moveInputValue.x > 0f;
  }

  private void OnDisable()
  {
    _jumpActionRef.action.Disable();
    _moveActionRef.action.Disable();
  }
}

