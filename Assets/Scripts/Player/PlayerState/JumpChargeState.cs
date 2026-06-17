using System;
using UnityEngine;

public class JumpChargeState : IState
{
  private PlayerController _player;
  private StateMachine _stateMachine;

  public JumpChargeState(PlayerController controller, StateMachine stateMachine)
  {
    _player = controller;
    _stateMachine = stateMachine;
  }

  public void OnEnter()
  {
    _player.JumpChargeTime = 0f;
    _player.ShowAnimation(AnimationConstants.JumpChargeTrigger);
  }

  public void FixedUpdate()
  {

  }

  public void Update()
  {
    _player.JumpChargeTime += Time.deltaTime;
    _player.JumpChargeTime = Mathf.Clamp(_player.JumpChargeTime, 0f, _player.JumpConfig.maxJumpChargeTime);

    bool isReleaseJumpPressed = !_player.PlayerInput.IsJumpPressed;
    bool isMaxChargeReached = _player.JumpChargeTime >= _player.JumpConfig.maxJumpChargeTime;

    if (isReleaseJumpPressed || isMaxChargeReached)
    {
      _stateMachine.TransitionTo(PlayerStateEnum.Jump);
    }
  }

  public void OnExit()
  {
    _player.HideAnimation(AnimationConstants.JumpChargeTrigger);
  }
}