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

  [Header("Audio")]
  [SerializeField] private FrogSound _frogSound;

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

  private bool _hasKey;
  public bool HasKey => _hasKey;

  public Action<Collision2D> OnPlayerCollision;
  public Action OnPlayerDead;

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

    if (collision.gameObject.CompareTag(TagConstant.DeadZone))
    {
      OnPlayerDead?.Invoke();
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag(TagConstant.Key))
    {
      _hasKey = true;
      collision.gameObject.SetActive(false);
    }

    if (collision.gameObject.CompareTag(TagConstant.Chest))
    {
      if (!_hasKey)
      {
        Debug.Log("Player does not have the key and cannot open the chest!");
        return;
      }

      Chest chest = collision.gameObject.GetComponent<Chest>();
      if (chest == null)
      {
        Debug.LogError("Chest component not found on the collided object!");
        return;
      }

      chest.OpenChest();
      Debug.Log("Player has the key and can open the chest!");
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

  public void PlayJumpSound()
  {
    _frogSound.PlayRandomSound();
  }

  public void PlayLandSound()
  {
    _frogSound.PlayRandomSound();
  }

  public void Reset()
  {
    _hasKey = false;
    _isGrounded = false;
    _isFacingRight = true;
    _lastJumpForce = Vector2.zero;
  }
}