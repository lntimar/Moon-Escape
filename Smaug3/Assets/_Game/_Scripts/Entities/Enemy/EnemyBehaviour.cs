using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] public bool isRange = true;

    [Header("Patrol:")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitTime;

    [Header("Collision Layers:")]
    [SerializeField] private CollisionLayers collisionLayers;

    [Header("Raycast:")]
    [SerializeField] private Vector2 rayDirection = Vector2.right;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask ignoreLayer;

    [Header("Chase:")]
    [SerializeField] private float minChaseDistance;
    [SerializeField] private float maxChaseDistance;
    [SerializeField] private float chaseSpeed;
    public bool chasing = false;

    [Header("Shoot:")]
    [SerializeField] private Transform shootSpawnPointLeft;
    [SerializeField] private Transform shootSpawnPointRight;
    public float initialShootInterval;
    public GameObject bullet;
    public bool shooting = false;

    // References
    private GameObject _player;
    private AudioManager _audioManager;

    // Components
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    private SpriteRenderer _spr;
    [HideInInspector] public Animator Anim;

    // Patrol
    private bool _canMove = true;
    private float _bcX;
    public float _horizontalDirection;

    // Raycast
    private float _curRayDistance;

    // Shoot Interval
    private float _shootInterval;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _spr = GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();

        _bcX = Mathf.Abs(_bc.offset.x);

        _player = GameObject.FindGameObjectWithTag("Player");

        _curRayDistance = rayDistance;

        _shootInterval = initialShootInterval;

        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, _curRayDistance, ~ignoreLayer);

        if (hit)
        {
            if (hit.collider.gameObject.layer == collisionLayers.PlayerLayer)
            {
                float dist = Vector2.Distance(transform.position, _player.transform.position);
                _curRayDistance = rayDistance;

                if (isRange) // Inimigo Range
                {
                    if (dist >= minChaseDistance)
                    {
                        chasing = true;
                        _canMove = false;                       
                    }
                    else
                    {
                        StopAllCoroutines();
                        chasing = false;
                        _canMove = false;
                        shooting = true;

                        // Shoot
                        if (_shootInterval <= 0)
                        {
                            Vector3 spawnPoint;

                            if (_spr.flipX) spawnPoint = shootSpawnPointLeft.transform.position;
                            else spawnPoint = shootSpawnPointRight.transform.position;

                            GameObject bulletObj = Instantiate(bullet, spawnPoint, Quaternion.identity);
                            var bulletScript = bulletObj.GetComponent<EnemyBulletMovement>();
                            Anim.SetTrigger("Shoot");
                            //_audioManager.PlaySFX("laser 1");
                            _shootInterval = initialShootInterval;

                            if (_spr.flipX)
                            {
                                bulletScript.moveDir = -1f;
                            }
                            else
                            {
                                bulletScript.moveDir = 1f;
                            }
                        }
                        else
                        {
                            _shootInterval -= Time.deltaTime;
                        }
                    }
                }
                else // Inimigo Melee
                {
                    chasing = true;
                    _canMove = false;
                }
            }
            else
            {
                _curRayDistance = Vector2.Distance(transform.position, hit.point);
                if (!chasing) _canMove = true;
            }

            Debug.DrawRay(transform.position, rayDirection * _curRayDistance, Color.green);
        }
        else
        {
            _curRayDistance = rayDistance;
            if (!chasing) _canMove = true;
            shooting = false;
        }

        if (_spr.flipX)
        {
            rayDirection.x = -1f;
            _bc.offset = new Vector2(-_bcX, _bc.offset.y);
            _horizontalDirection = -1f;
        }
        else
        {
            rayDirection.x = 1f;
            _bc.offset = new Vector2(_bcX, _bc.offset.y);
            _horizontalDirection = 1f;
        }

        Anim.SetFloat("Move", Mathf.Abs(_rb.velocity.x));
    }

    private void FixedUpdate()
    { 
        if (chasing)
        { 
            _horizontalDirection = Mathf.Sign(_player.transform.position.x - transform.position.x);
            _rb.velocity = new Vector2(_horizontalDirection * chaseSpeed, _rb.velocity.y);

            if (Vector2.Distance(transform.position, _player.transform.position) >= maxChaseDistance)
            {
                chasing = false;
            }

            // Fix do Flick dos inimigos
            if (Vector2.Distance(transform.position, _player.transform.position) < 1f)
            {
                _rb.velocity = Vector2.zero;
            }

            if (_rb.velocity.x < 0.01f) _spr.flipX = true;
            else if (_rb.velocity.x > 0.01f) _spr.flipX = false;
        }
     
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
        if (_spr.flipX)
        {
            _spr.flipX = false;
            _horizontalDirection = 1f;
        }
        else
        {
            _spr.flipX = true;
            _horizontalDirection = -1f;
        }
    }

    public IEnumerator EnemyWaits()
    {
        _canMove = false;
        yield return new WaitForSeconds(waitTime);
        _canMove = true;
        Flip();
    }

    public void StartMovement()
    {
        if (Random.Range(0f, 100f) < 50f) _spr.flipX = true;
        else _spr.flipX = false;

        if (_spr.flipX)
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
}