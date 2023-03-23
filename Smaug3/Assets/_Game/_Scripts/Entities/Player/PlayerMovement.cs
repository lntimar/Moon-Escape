using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement:")]
    public float MoveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private bool isLocked = false;

    [Header("Jump:")]
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isJumped = false;

    [Header("Ground Check:")]
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private CollisionLayers collisionLayers;

    [Header("Coyote Time:")]
    [SerializeField, Range(0f, 2f)] private float coyoteTime;
    [SerializeField] private float coyoteTimeCounter;

    // Components
    private Rigidbody2D _rb;
    private SpriteRenderer _spr;
    private Animator _anim;
    private PlayerShoot _playerShoot;

    // Inputs
    private float _dirH;
    private float _dirV;
    

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _playerShoot = GetComponent<PlayerShoot>();
    }

    private void Update()
    {
        // Não execute a verificação de input, caso o player estiver morto ou ter tomado dano
        if (PlayerStateMachine.StateManager.IsNotFine()) return;

        _anim.SetBool("isGrounded", isGrounded);

        MoveInput();
        JumpInput();
        LockMove();

        FlipX();
    }

    private void FixedUpdate()
    {
        // Não aplique algum tipo de física, caso o player estiver morto ou ter tomado dano
        if (PlayerStateMachine.StateManager.IsNotFine()) return;

        CoyoteTime();

        ApplyMove();
        ApplyJump();
    }

    // Checando chão
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.GroundLayer)
        {
            isGrounded = true;
            isJumped = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.GroundLayer)
        {
            isGrounded = false;
        }
    }

    private void MoveInput()
    {
        // Apenas pega um número entre -1, 0, 1 e guarda
        _dirH = Input.GetAxisRaw("Horizontal");
        _dirV = Input.GetAxisRaw("Vertical");

        // Direção que o player está mirando
        var shootDir = new Vector2(_dirH, _dirV).normalized;

        if (shootDir.x != 0f && shootDir.y != 0f)
        {
            _playerShoot.SetDirection(shootDir);

            if (shootDir.y > 0f) _playerShoot.SetShootAnim("Diagonal Up");
            else _playerShoot.SetShootAnim("Diagonal Down");
        }
        else
        {
            if (shootDir == Vector2.zero)
            {
                if (_spr.flipX) _playerShoot.SetDirection(Vector2.left);
                else _playerShoot.SetDirection(Vector2.right);

                _playerShoot.SetShootAnim("Horizontal");
            }
            else if (shootDir.x != 0)
            {
                if (shootDir.x > 0) _playerShoot.SetDirection(Vector2.right);
                else _playerShoot.SetDirection(Vector2.left);
                _playerShoot.SetShootAnim("Horizontal");
            }
            else
            {
                if (shootDir.y > 0)
                {
                    _playerShoot.SetDirection(Vector2.up);
                    _playerShoot.SetShootAnim("Vertical Up");
                }
                else
                {
                    _playerShoot.SetDirection(Vector2.down);
                    _playerShoot.SetShootAnim("Vertical Down");
                }
            }
        }

        // Animator
        if (!isLocked) _anim.SetFloat("move", Mathf.Abs(_dirH));
        else _anim.SetFloat("move", 0f);
    }

    private void LockMove()
    {
        if (Input.GetButton("Lock Player")) isLocked = true;

        if (Input.GetButtonUp("Lock Player")) isLocked = false;
    }

    private float SetVelocity(float goal, float curVelocity, float accel)
    {
        var velDifference = goal - curVelocity;

        if (velDifference > accel) return curVelocity + accel;
        if (velDifference < -accel) return curVelocity - accel;

        return goal;
    }

    private void ApplyMove()
    {
        if (isLocked)
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            return;
        }

        _rb.velocity = new Vector2(SetVelocity(MoveSpeed * _dirH, _rb.velocity.x, acceleration), _rb.velocity.y);

        if (_rb.velocity.x != 0) PlayerStateMachine.StateManager.SetState(PlayerStateMachine.PlayerStates.Moving);
        else PlayerStateMachine.StateManager.SetState(PlayerStateMachine.PlayerStates.Idle);
    }

    private void JumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                isJumping = true;
            }
            else if (coyoteTimeCounter > 0f)
            {
                isJumping = true;
                coyoteTimeCounter = 0f;
            }
        }
    }

    private void ApplyJump()
    {
        if (isJumping)
        {
            isJumping = false;
            isJumped = true;
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);
            _rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            _anim.SetTrigger("jump");
        }
    }

    private void CoyoteTime()
    {
        if (!isGrounded && _rb.velocity.y < 0.01f && !isJumped)
        {
            coyoteTimeCounter = coyoteTime;
        }

        if (coyoteTimeCounter > 0f)
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            coyoteTimeCounter = 0f;
        }
    }

    private void FlipX()
    {
        if (_dirH == -1f) _spr.flipX = true;
        else if (_dirH == 1f) _spr.flipX = false;
    }
}
