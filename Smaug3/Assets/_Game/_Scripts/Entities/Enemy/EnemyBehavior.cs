using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitTime;
    [SerializeField] private CollisionLayers collisionLayers;
    
    // Components
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    private SpriteRenderer _spr;

    private float _horizontalDirection;

    private bool _canMove = true;

    private float _bcX;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _spr = GetComponent<SpriteRenderer>();

        _bcX = Mathf.Abs(_bc.offset.x);
    }

    private void FixedUpdate()
    {
        if (_canMove) Patrol();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.GroundLayer)
        {
            StartMovement();
        }
        else if (col.gameObject.layer == collisionLayers.WallLayer)
        {
            StartCoroutine(EnemyWaits());
        }
        else if (col.gameObject.layer == collisionLayers.EnemyLayer)
        {
            Flip();
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.GroundLayer)
        {
            StartCoroutine(EnemyWaits());
        }
    }
    
    //Movimento de patrulha andando da direita pra esquerda
    private void Patrol()
    {
        _rb.velocity = new Vector2(moveSpeed * _horizontalDirection * Time.fixedDeltaTime, _rb.velocity.y);
    }

    private void Flip()
    {
        _spr.flipX = !_spr.flipX;
        if (_spr.flipX == true)
        {
            _bc.offset = new Vector2(-_bcX, _bc.offset.y);
            _horizontalDirection = -1f;
        }
        else
        {
            _bc.offset = new Vector2(_bcX, _bc.offset.y);
            _horizontalDirection = 1f;
        }
    }

    //speedValue vai armazenar o valor da speed antes que ele seja zerado
    private IEnumerator EnemyWaits()
    {
        _canMove = false;
        yield return new WaitForSeconds(waitTime);
        Flip();
        _canMove = true;
    }

    public void StartMovement()
    {
        if (Random.Range(0f, 100f) < 50f) _spr.flipX = true;
        else _spr.flipX = false;

        if (_spr.flipX == true)
        {
            _bc.offset = new Vector2(-_bcX, _bc.offset.y);
            _horizontalDirection = -1f;
        }
        else
        {
            _bc.offset = new Vector2(_bcX, _bc.offset.y);
            _horizontalDirection = 1f;
        }
    }

    public void SetMovement(bool enable)
    {
        _canMove = enable;
        if (enable == false) _rb.velocity = new Vector2(0, _rb.velocity.y);
    }
}