using UnityEngine;

public static class AnimationConstants
{
  public const string IdleTrigger = "idle";
  public const string JumpChargeTrigger = "jumpCharge";
  public const string JumpTrigger = "jump";
  public const string FallTrigger = "fall";
  public const string LandTrigger = "land";
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerView : MonoBehaviour
{
  private Animator _animator;

  private void Awake()
  {
    _animator = GetComponent<Animator>();
  }

  public void ShowAnimation(string trigger)
  {
    _animator.SetTrigger(trigger);
  }

  public void HideAnimation(string trigger)
  {
    _animator.ResetTrigger(trigger);
  }
}