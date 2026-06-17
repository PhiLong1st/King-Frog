using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateMachine
{

  public IState CurrentState => _states[_stateKey];
  public event Action<IState> stateChanged;

  private PlayerStateEnum _stateKey;
  private Dictionary<PlayerStateEnum, IState> _states;

  public StateMachine(PlayerController player)
  {
    var idleState = new IdleState(player, this);
    var jumpChargeState = new JumpChargeState(player, this);
    var jumpState = new JumpState(player, this);

    _states = new Dictionary<PlayerStateEnum, IState>
    {
      [PlayerStateEnum.Idle] = idleState,
      [PlayerStateEnum.JumpCharge] = jumpChargeState,
      [PlayerStateEnum.Jump] = jumpState
    };

    _stateKey = PlayerStateEnum.Idle;
    CurrentState.OnEnter();
    stateChanged?.Invoke(idleState);
  }

  public void TransitionTo(PlayerStateEnum nextState)
  {
    CurrentState.OnExit();
    _stateKey = nextState;

    CurrentState.OnEnter();
    stateChanged?.Invoke(CurrentState);
  }

  public void FixedUpdate()
  {
    if (CurrentState is null)
    {
      return;
    }

    CurrentState.FixedUpdate();
  }

  public void Update()
  {
    if (CurrentState is null)
    {
      return;
    }

    CurrentState.Update();
    Debug.Log($"Current State: {_stateKey}");
  }
}