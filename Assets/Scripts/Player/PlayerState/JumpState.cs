using System;
using UnityEngine;

public class JumpState : IState
{
  private PlayerController _player;
  private StateMachine _stateMachine;

  public JumpState(PlayerController controller, StateMachine stateMachine)
  {
    _player = controller;
    _stateMachine = stateMachine;
  }

  private bool _isAppliedFallAnimation = false;

  public void OnEnter()
  {
    _player.ShowAnimation(AnimationConstants.JumpTrigger);

    float jumpForce = CalculateJumpForce();
    Vector2 jumpDirection = CalculateJumpDirection();
    Vector2 jumpVelocity = jumpDirection * jumpForce;
    _player.Jump(jumpVelocity);

    _player.OnPlayerCollision += HandleCollision;
    _isAppliedFallAnimation = false;
  }

  public void FixedUpdate()
  {

  }

  public void Update()
  {
    if (_player.IsFalling && !_isAppliedFallAnimation)
    {
      _player.HideAnimation(AnimationConstants.JumpTrigger);
      _player.ShowAnimation(AnimationConstants.FallTrigger);
      _isAppliedFallAnimation = true;
    }

    if (_player.IsGrounded)
    {
      _stateMachine.TransitionTo(PlayerStateEnum.Idle);
    }
  }

  private void HandleCollision(Collision2D collision)
  {
    if (!collision.gameObject.CompareTag(TagConstant.Wall))
    {
      return;
    }

    if (_player.IsFalling)
    {
      return;
    }

    ApplyWallJump();
  }

  private void ApplyWallJump()
  {
    var jumpVelocity = new Vector2(-_player.LastJumpForce.x, _player.LastJumpForce.y);
    _player.Flip();
    _player.Jump(jumpVelocity);
  }

  private float CalculateJumpForce()
  {
    return _player.JumpConfig.jumpForce + (_player.JumpChargeTime * _player.JumpConfig.jumpForceMultiplier);
  }

  private Vector2 CalculateJumpDirection()
  {
    int xDirection = _player.IsFacingRight ? 1 : -1;
    Vector2 jumpDirection = new Vector2(xDirection, 1);
    return jumpDirection;
  }

  public void OnExit()
  {
    _player.StopMovement();
    _player.OnPlayerCollision -= HandleCollision;
    // _player.HideAnimation(AnimationConstants.JumpTrigger);
    _player.HideAnimation(AnimationConstants.FallTrigger);
  }
}