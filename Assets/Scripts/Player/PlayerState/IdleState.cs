using System;
using UnityEngine;

public class IdleState : IState
{
  private PlayerController _player;
  private StateMachine _stateMachine;

  public IdleState(PlayerController controller, StateMachine stateMachine)
  {
    _player = controller;
    _stateMachine = stateMachine;
  }

  public void OnEnter()
  {
    _player.ShowAnimation(AnimationConstants.IdleTrigger);
  }

  public void FixedUpdate()
  {

  }

  public void Update()
  {
    if (_player.PlayerInput.IsMoveLeftPressed)
    {
      HandleFlipLeft();
    }
    else if (_player.PlayerInput.IsMoveRightPressed)
    {
      HandleFlipRight();
    }
    else if (_player.PlayerInput.IsJumpPressed)
    {
      HandleJumpPressed();
    }
  }

  private void HandleFlipLeft()
  {
    if (!_player.IsFacingRight)
    {
      return;
    }

    _player.Flip();
  }

  private void HandleFlipRight()
  {
    if (_player.IsFacingRight)
    {
      return;
    }

    _player.Flip();
  }

  private void HandleJumpPressed()
  {
    _stateMachine.TransitionTo(PlayerStateEnum.JumpCharge);
  }

  public void OnExit()
  {
    _player.HideAnimation(AnimationConstants.IdleTrigger);
  }
}