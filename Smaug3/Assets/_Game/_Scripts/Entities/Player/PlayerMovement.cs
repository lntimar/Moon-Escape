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

    [Header("Dash:")]
    [SerializeField] private float dashForce; 
    [SerializeField] private float dashTime;
    [SerializeField] private float dashInterval;
    [SerializeField] private float dashEffectInterval;
    [SerializeField] private GameObject dashEffectPrefab;

    // Components
    private Rigidbody2D _rb;
    private SpriteRenderer _spr;
    private Animator _anim;
    private PlayerShoot _playerShoot;

    // Inputs
    private float _dirH;
    private float _dirV;
    
    // Dash
    public static bool HasDash = true;
    private bool _canDash = true;
    private bool _applyingDashEffect = false;
    private Color[] dashColors = { Color.green, Color.cyan, Color.magenta, Color.yellow};

    // Gravity
    private float _rbGravity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _playerShoot = GetComponent<PlayerShoot>();

        _rbGravity = _rb.gravityScale;
    }

    private void Update()
    {
        // Não aplique algum tipo de física, caso o player estiver morto ou ter tomado dano / carregando
        if (PlayerStateMachine.StateManager.IsNotFine() 
            || PlayerStateMachine.StateManager.CompareState(PlayerStateMachine.PlayerStates.Loading))
            return;

        if (!PlayerStateMachine.StateManager.CompareState(PlayerStateMachine.PlayerStates.Dashing))
        {
            _anim.SetBool("isGrounded", isGrounded);
            MoveInput();
            JumpInput();
            LockMove();
        }
        else
        {
            _anim.SetBool("isGrounded", false);
        }

        // Caso tiver o upgrade do Dash
        if (HasDash)
        {
            DashInput();
            if (PlayerStateMachine.StateManager.CompareState(PlayerStateMachine.PlayerStates.Dashing))
            {
                if (!_applyingDashEffect)
                {
                    StartDashEffect();
                    _applyingDashEffect = true;
                }
            }
        }

        FlipX();
    }

    private void FixedUpdate()
    {
        // Não aplique algum tipo de física, caso o player estiver morto ou ter tomado dano / carregando
        if (PlayerStateMachine.StateManager.IsNotFine() ||
            PlayerStateMachine.StateManager.CompareState(PlayerStateMachine.PlayerStates.Loading))
            return;

        // Caso não estiver em um Dash
        if (!PlayerStateMachine.StateManager.CompareState(PlayerStateMachine.PlayerStates.Dashing))
        {
            CoyoteTime();
            ApplyMove();
            ApplyJump();
        }
        else // Se está em um Dash
        {
            if (Mathf.Abs(_rb.velocity.x) < dashForce) ApplyDash();
        }
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

    private void DashInput()
    {
        if (Input.GetButtonDown("Dash") && _canDash)
        {
            _canDash = false;
            _rb.gravityScale = 0f;
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);
            PlayerStateMachine.StateManager.SetState(PlayerStateMachine.PlayerStates.Dashing);
            Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.BossLayer, true);
            Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.EnemyLayer, true);
            StartCoroutine(SetDashTime(dashTime));
        }
    }

    private IEnumerator SetDashTime(float t)
    {
        yield return new WaitForSeconds(t);
        PlayerStateMachine.StateManager.SetState(PlayerStateMachine.PlayerStates.Idle);
        Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.BossLayer, false);
        Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.EnemyLayer, false);
        _rb.gravityScale = _rbGravity;
        StartCoroutine(SetDashInterval(dashInterval));
    }

    private void ApplyDash()
    {
        if (_dirH != 0 || _dirV != 0)
        {
            if (_dirH == 0 && _dirV != 0) 
                _rb.AddForce(new Vector2(_dirH, _dirV).normalized * (dashForce * 0.25f), ForceMode2D.Impulse);
            else
                _rb.AddForce(new Vector2(_dirH, _dirV).normalized * dashForce, ForceMode2D.Impulse);
        }
        else
        {
            if (_spr.flipX) _rb.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
            else _rb.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
        }
    }

    private void StartDashEffect()
    {
        StartCoroutine(ApplyDashEffect(0.02f));
        StartCoroutine(StopDashEffect(dashEffectInterval));
    }

    private IEnumerator ApplyDashEffect(float t)
    {
        yield return new WaitForSeconds(t);
        var effect = Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);
        var effectSpr = effect.GetComponent<SpriteRenderer>();
        effectSpr.flipX = _spr.flipX;
        effectSpr.color = dashColors[Random.Range(0, dashColors.Length - 1)];
        if (_applyingDashEffect) StartCoroutine(ApplyDashEffect(0.02f));
    }

    private IEnumerator StopDashEffect(float t)
    {
        yield return new WaitForSeconds(dashEffectInterval);
        _applyingDashEffect = false;
    }

    private IEnumerator SetDashInterval(float t)
    {
        yield return new WaitForSeconds(t);
        _canDash = true;
    }

    private void FlipX()
    {
        if (_dirH == -1f) _spr.flipX = true;
        else if (_dirH == 1f) _spr.flipX = false;
    }
}
