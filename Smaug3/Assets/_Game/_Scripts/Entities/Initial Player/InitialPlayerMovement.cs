using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class InitialPlayerMovement : MonoBehaviour
{
    [Header("Movement:")] 
    public float MoveSpeed;
    [SerializeField] private float acceleration;

    [Header("Jump:")] 
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isJumped = false;

    [Header("Ground Check:")] 
    [SerializeField] public bool isGrounded = false;
    [SerializeField] private CollisionLayers collisionLayers;

    [Header("Coyote Time:")]
    [SerializeField, Range(0f, 6f)] private float coyoteTime;
    [SerializeField] private float coyoteTimeCounter = 0;

    // Components
    private Rigidbody2D _rb;
    private SpriteRenderer _spr;
    private Animator _anim;
    private InitialPlayerAttack _playerAttack;

    // Input
    private float _dirH;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _playerAttack = GetComponent<InitialPlayerAttack>();
    }

    private void Update()
    {
        // Não execute a verificação de input, caso o player inicial estiver morto / ter tomado dano / está atacando
        if (InitialPlayerStateMachine.StateManager.IsNotFine() || _playerAttack.IsAttacking) return;

        _anim.SetBool("isGrounded", isGrounded);

        MoveInput();
        JumpInput();
        
        FlipX();
    }

    private void FixedUpdate()
    {
        // Não aplique algum tipo de física, caso o player inicial estiver morto / ter tomado dano / está atacando
        if (InitialPlayerStateMachine.StateManager.IsNotFine() || _playerAttack.IsAttacking) return;

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

        _anim.SetFloat("move", Mathf.Abs(_dirH));
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
        _rb.velocity = new Vector2(SetVelocity(MoveSpeed * _dirH, _rb.velocity.x, acceleration), _rb.velocity.y);

        if (_rb.velocity.x != 0) InitialPlayerStateMachine.StateManager.SetState(InitialPlayerStateMachine.InitialPlayerStates.Moving);
        else InitialPlayerStateMachine.StateManager.SetState(InitialPlayerStateMachine.InitialPlayerStates.Idle);
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