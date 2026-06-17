using UnityEngine;

public enum PlayerAnimation
{
  Idle,
  JumpCharge,
  Jump,
  Fall
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerView : MonoBehaviour
{
  private const string JumpTrigger = "jump";
  private const string JumpChargeTrigger = "jumpCharge";
  private const string AttackTrigger = "attack";

  private const string IdleTrigger = "idle";
  private const string FallTrigger = "fall";
  private const string LandTrigger = "land";

  private SpriteRenderer _spriteRenderer;
  private Animator _animator;

  private string _lastAnimationTrigger;

  private void Awake()
  {
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _animator = GetComponent<Animator>();
  }

  private void Update()
  {
  }

  private void ShowAttackAnimation()
  {
    _animator.ResetTrigger(_lastAnimationTrigger);
    _animator.SetTrigger(AttackTrigger);
    _lastAnimationTrigger = AttackTrigger;
  }

  private void ShowJumpChargeAnimation()
  {
    _animator.ResetTrigger(_lastAnimationTrigger);
    _animator.SetTrigger(JumpChargeTrigger);
    _lastAnimationTrigger = JumpChargeTrigger;
  }

  private void UnshowJumpChargeAnimation()
  {
    _animator.ResetTrigger(_lastAnimationTrigger);
    _animator.SetTrigger(IdleTrigger);
    _lastAnimationTrigger = IdleTrigger;
  }

  private void ShowJumpAnimation()
  {
    _animator.ResetTrigger(_lastAnimationTrigger);
    _animator.SetTrigger(AttackTrigger);
    _lastAnimationTrigger = AttackTrigger;
  }
}