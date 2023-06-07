using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPlayerAttack : MonoBehaviour
{
    [Header("Attack:")]
    public int damage;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private BoxCollider2D hitbox;
    [SerializeField] private SpriteRenderer attackSprite;
    [SerializeField] private Animator attackAnim;

    // References
    private AudioManager _audioManager;

    // Components
    private Rigidbody2D _rb;
    private SpriteRenderer _spr;

    [HideInInspector] public bool IsAttacking = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();

        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (InitialPlayerStateMachine.StateManager.IsNotFine()) return;

        AttackInput();
        VerifyIsAttacking();
    }

    private void AttackInput()
    {
        if(canAttack)
        {
            if (Input.GetButtonDown("Shoot Mouse"))
            {
                canAttack = false;
                attackAnim.SetTrigger("attack");
                _audioManager.PlaySFX("macaquinho caindo");
            }
            else if (Input.GetButtonDown("Shoot Key"))
            {
                canAttack = false;
                attackAnim.SetTrigger("attack");
                _audioManager.PlaySFX("macaquinho caindo");
            }
        }
    }

    private void VerifyIsAttacking()
    {
        if (attackAnim.GetCurrentAnimatorStateInfo(0).IsName("Initial Player Attack Animation"))
        {
            _spr.enabled = false;
            attackSprite.enabled = true;
            attackSprite.flipX = _spr.flipX;
            hitbox.enabled = true;
            IsAttacking = true;

            _rb.velocity = new Vector2(0f, _rb.velocity.y);
        }
        else
        {
            _spr.enabled = true;
            attackSprite.enabled = false;
            hitbox.enabled = false;
            IsAttacking = false;
            canAttack = true;
        }
    }
}
