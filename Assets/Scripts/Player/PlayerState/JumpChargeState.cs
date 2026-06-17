using System;
using UnityEngine;

public class JumpChargeState : IState
{
  private PlayerController _player;
  private StateMachine _stateMachine;
  private float _jumpChargeTime;

  public JumpChargeState(PlayerController controller, StateMachine stateMachine)
  {
    _player = controller;
    _stateMachine = stateMachine;
  }

  public void OnEnter()
  {
    _jumpChargeTime = 0f;
    _player.ShowAnimation(PlayerAnimation.JumpCharge);
  }

  public void FixedUpdate()
  {

  }

  public void Update()
  {
    _jumpChargeTime += Time.deltaTime;
    _jumpChargeTime = Mathf.Clamp(_jumpChargeTime, 0f, _player.JumpConfig.maxJumpChargeTime);

    bool isReleaseJumpPressed = !_player.PlayerInput.IsJumpPressed;
    bool isMaxChargeReached = _jumpChargeTime >= _player.JumpConfig.maxJumpChargeTime;

    if (isReleaseJumpPressed || isMaxChargeReached)
    {
      float jumpForce = CalculateJumpForce();
      Vector2 jumpDirection = CalculateJumpDirection();
      Vector2 jumpVelocity = jumpDirection * jumpForce;

      _player.Jump(jumpVelocity);
      _stateMachine.TransitionTo(PlayerStateEnum.Jump);
    }
  }

  public void OnExit()
  {
    _player.HideAnimation(PlayerAnimation.JumpCharge);
  }

  private float CalculateJumpForce()
  {
    return _player.JumpConfig.jumpForce + (_jumpChargeTime * _player.JumpConfig.jumpForceMultiplier);
  }

  private Vector2 CalculateJumpDirection()
  {
    int xDirection = _player.IsFacingRight ? 1 : -1;
    Vector2 jumpDirection = new Vector2(xDirection, 1);
    return jumpDirection;
  }
}