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

  public void OnEnter()
  {
    _player.ShowAnimation(PlayerAnimation.Jump);

    _player.OnPlayerCollision += HandleCollision;
  }

  public void FixedUpdate()
  {

  }

  public void Update()
  {
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

  public void OnExit()
  {
    _player.StopMovement();
    _player.HideAnimation(PlayerAnimation.Jump);
    _player.OnPlayerCollision -= HandleCollision;
  }
}