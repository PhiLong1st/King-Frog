using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  [Header("Input Actions")]
  [SerializeField] private PlayerInput _playerInput;

  [Header("Jump Config")]
  [SerializeField] private JumpConfig _jumpConfig;

  [Header("Frog States")]
  [SerializeField] private bool _isGrounded;
  [SerializeField] private bool _isFacingRight;

  public PlayerInput PlayerInput => _playerInput;
  public JumpConfig JumpConfig => _jumpConfig;
  public bool IsGrounded => _isGrounded;
  public bool IsFacingRight => _isFacingRight;
  public bool IsFalling => !_isGrounded && _rigidbody.linearVelocity.y < 0;

  private Rigidbody2D _rigidbody;
  private PlayerView _view;
  private StateMachine _stateMachine;

  public float JumpChargeTime { get; set; }

  private Vector2 _lastJumpForce;
  public Vector2 LastJumpForce => _lastJumpForce;

  public Action<Collision2D> OnPlayerCollision;

  private void Awake()
  {
    _rigidbody = GetComponent<Rigidbody2D>();
    _view = GetComponentInChildren<PlayerView>();
    _playerInput = GetComponent<PlayerInput>();
  }

  private void Start()
  {
    _isGrounded = false;
    _isFacingRight = true;
    _stateMachine = new StateMachine(this);
    _lastJumpForce = Vector2.zero;
  }

  public void Flip()
  {
    _isFacingRight = !_isFacingRight;

    Vector3 localScale = transform.localScale;
    localScale.x *= -1;
    transform.localScale = localScale;
  }

  private void FixedUpdate()
  {
    _stateMachine.FixedUpdate();
  }

  private void Update()
  {
    _stateMachine.Update();
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    OnPlayerCollision?.Invoke(collision);

    if (collision.gameObject.CompareTag(TagConstant.Ground))
    {
      _isGrounded = true;
    }
  }

  public void Jump(Vector2 jumpVelocity)
  {
    _isGrounded = false;
    _lastJumpForce = jumpVelocity;
    _rigidbody.AddForce(jumpVelocity, ForceMode2D.Impulse);
  }

  public void ShowAnimation(string trigger)
  {
    _view.ShowAnimation(trigger);
  }

  public void HideAnimation(string trigger)
  {
    _view.HideAnimation(trigger);
  }

  public void StopMovement()
  {
    _rigidbody.linearVelocity = Vector2.zero;
  }
}