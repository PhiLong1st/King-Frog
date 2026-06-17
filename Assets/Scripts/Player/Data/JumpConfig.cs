using UnityEngine;

[CreateAssetMenu(fileName = "JumpConfig", menuName = "Frog/JumpConfig", order = 0)]
public class JumpConfig : ScriptableObject
{
  public float jumpForce = 1f;
  public float jumpForceMultiplier = 1f;
  public float maxJumpChargeTime = 4f;
}