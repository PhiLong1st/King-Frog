using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  [SerializeField] private InputActionReference _jumpActionRef;
  [SerializeField] private InputActionReference _moveActionRef;
  [SerializeField] private JumpConfig _jumpConfig;

  private Rigidbody2D _rigidbody;

  [SerializeField] private bool _isGrounded;
  [SerializeField] private bool _isFacingRight;
  [SerializeField] private bool _isJumpCharging;
  [SerializeField] private float _jumpChargeTime = 0f;

  private float _lastJumpForce;

  private void Awake()
  {
    _rigidbody = GetComponent<Rigidbody2D>();
  }

  private void Start()
  {
    _isFacingRight = true;
    _isGrounded = false;
    _isJumpCharging = false;
    _jumpChargeTime = 0f;
    _lastJumpForce = 0f;
  }

  private void OnEnable()
  {
    _jumpActionRef.action.Enable();
    _moveActionRef.action.Enable();
  }

  private void OnDisable()
  {
    _jumpActionRef.action.Disable();
    _moveActionRef.action.Disable();
  }

  private void Flip()
  {
    _isFacingRight = !_isFacingRight;

    Vector3 localScale = transform.localScale;
    localScale.x *= -1;
    transform.localScale = localScale;
  }

  private void Update()
  {
    if (_moveActionRef.action.IsPressed())
    {
      Vector2 moveInput = _moveActionRef.action.ReadValue<Vector2>();
      float horizontalMove = moveInput.x;

      if (horizontalMove > 0 && !_isFacingRight)
      {
        Flip();
      }
      else if (horizontalMove < 0 && _isFacingRight)
      {
        Flip();
      }
    }

    if (!_isGrounded)
    {
      return;
    }

    bool isJumpPressed = _jumpActionRef.action.IsPressed();

    if (isJumpPressed)
    {
      OnChargingJump();
    }
    else if (_isJumpCharging)
    {
      OnJump();
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag(TagConstant.Ground))
    {
      _isGrounded = true;
      _rigidbody.linearVelocity = Vector2.zero;
    }

    if (collision.gameObject.CompareTag(TagConstant.Wall))
    {
      Flip();
      Jump(_lastJumpForce);
    }
  }

  private void OnCollisionExit2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag(TagConstant.Ground))
    {
      _isGrounded = false;
    }
  }

  private void Jump(float jumpForce)
  {
    _lastJumpForce = jumpForce;

    Vector2 direction = CalculateJumpDirection();
    _rigidbody.AddForce(direction * jumpForce, ForceMode2D.Impulse);
  }

  private void OnChargingJump()
  {
    _isJumpCharging = true;
    _jumpChargeTime += Time.deltaTime;
    _jumpChargeTime = Mathf.Clamp(_jumpChargeTime, 0f, _jumpConfig.maxJumpChargeTime);
  }

  private void OnJump()
  {
    var jumpForce = CalculateJumpForce();
    Jump(jumpForce);
    Reset();
  }

  private float CalculateJumpForce()
  {
    return _jumpConfig.jumpForce + (_jumpChargeTime * _jumpConfig.jumpForceMultiplier);
  }

  private Vector2 CalculateJumpDirection()
  {
    int xDirection = _isFacingRight ? 1 : -1;
    Vector2 jumpDirection = new Vector2(xDirection, 1);
    return jumpDirection;
  }

  private void Reset()
  {
    _isJumpCharging = false;
    _jumpChargeTime = 0f;
  }
}

/**
 * Prevent double jump: done
 * Detect ground contact: done
 * Variable jump height
 * Add jump charge time and multiplier to increase jump force: done
 * Add jump direction based on facing direction: 
 */
