using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorilaBombBehaviour : MonoBehaviour
{
    [Header("Movement:")]
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float flipXInterval;

    [Header("Start Animation:")]
    [SerializeField] private float minWaitingInterval;
    [SerializeField] private float maxWaitingInterval;

    [Header("Attack:")]
    [SerializeField] private Transform attackTransform;

    [Header("Shoot:")] 
    [SerializeField] private Bomb bombPrefab;
    [SerializeField] private Transform[] moveShootPoints;
    [SerializeField] private Transform bombSpawnPoint;
    [SerializeField] private int bombMaxCount;
    [SerializeField] private float bombInterval;

    [Header("Collision Layers:")]
    [SerializeField] private CollisionLayers collisionLayers;

    // References
    private GameObject _player;
    private AudioManager _audioManager;

    // Components
    private SpriteRenderer _spr;
    private Animator _anim;
    private Rigidbody2D _rb;
    private Boss _bossScript;

    // Movement
    private bool _canMove = false;
    private int _selectedPoint;

    // Movement Shoot
    private bool _canMoveShoot = false;
    private int _selectedShootPoint;

    // Start Anim
    private bool _isWaiting = false;
    private bool _canChange = false;

    // Shoot
    private int _bombSelectedCount;
    private int _bombCurCount = 1;
    private bool _isShooting = false;

    public enum BombGorilaActions
    {
        Start,
        Move,
        Attack,
        MoveShoot,
        Shoot
    }

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _bossScript = GetComponent<Boss>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        StartCoroutine(FlipX(flipXInterval));

        _audioManager.PlayMusic("gorila bomb");
    }

    private void Update()
    {
        if (_canMove)
        {
            Move();
        }

        if (_canMoveShoot)
        {
            _rb.gravityScale = 0f;
            _rb.drag = 0;
            _rb.isKinematic = true;
            MoveShoot();
        }

        if (!_isShooting && Vector2.Distance(transform.position, _player.transform.position) <= 6f)
        {
            if (Random.Range(0, 100) < 5)
            {
                _anim.Play("Gorila Bomb Attack Animation");
                _isWaiting = false;
                _canChange = false;
            }
        }
    }

    public void ChooseNextAction(BombGorilaActions currentAction)
    {
        if (currentAction == BombGorilaActions.Start)
        {
            // Espere para trocar para a ação de mover
            if (!_isWaiting)
            {
                StartCoroutine(StartChangeInterval(minWaitingInterval, maxWaitingInterval));
                _isWaiting = true;
            }

            // Se já tiver passado o tempo de espera
            if (_canChange)
            {
                _rb.gravityScale = 0f;
                _rb.drag = 0f;
                if (Random.Range(0, 100) < 50)
                {
                    _anim.Play("Gorila Bomb Move Animation");
                    EnableMove();
                }
                else
                {
                    _anim.Play("Gorila Bomb MoveShoot Animation");
                    EnableMoveShoot();
                }

                // Reinicie para a próxima vez
                _canChange = false;
                _isWaiting = false;
            }
        }
        else if (currentAction == BombGorilaActions.Move)
        {
            _anim.Play("Gorila Bomb Attack Animation");
        }
        else if (currentAction == BombGorilaActions.Attack)
        {
            _anim.Play("Gorila Bomb Start Animation");
        }
        else if (currentAction == BombGorilaActions.MoveShoot)
        {
            _spr.flipX = false;

            if (_selectedShootPoint == 0)
                _anim.Play("Gorila Bomb Shoot Right Animation");
            else
                _anim.Play("Gorila Bomb Shoot Left Animation");

            _bombSelectedCount = Random.Range(1, bombMaxCount + 1);
            StartCoroutine(SpawnBomb(bombInterval));
            _isShooting = true;
        }
        else // Shoot
        {
            _rb.isKinematic = false;
            _isShooting = false;
            _anim.Play("Gorila Bomb Move Animation");
            EnableMove(true);
        }
    }

    public void EnableMove(bool centralPoint=false)
    {
        _canMove = true;

        int newPoint;

        if (!centralPoint)
        {
            newPoint = Random.Range(0, movePoints.Length);
            while (_selectedPoint == newPoint && newPoint == movePoints.Length - 1)
            {
                newPoint = Random.Range(0, movePoints.Length);
            }
        }
        else
        {
            newPoint = movePoints.Length - 1;
        }

        _selectedPoint = newPoint;
    }

    private void Move()
    {
        var newPos = Vector3.Lerp(transform.position, movePoints[_selectedPoint].position, _bossScript.Speed * Time.deltaTime);
        transform.position = newPos;

        if (Vector2.Distance(transform.position, movePoints[_selectedPoint].position) <= 1f)
        {
            _canMove = false;
            ChooseNextAction(BombGorilaActions.Move);
        }
    }

    public void EnableMoveShoot()
    {
        _canMoveShoot = true;

        int newPoint = Random.Range(0, moveShootPoints.Length);
        while (_selectedShootPoint == newPoint)
        {
            newPoint = Random.Range(0, moveShootPoints.Length);
        }

        _selectedShootPoint = newPoint;
    }

    private void MoveShoot()
    {
        var newPos = Vector3.Lerp(transform.position, moveShootPoints[_selectedShootPoint].position, _bossScript.Speed * Time.deltaTime);
        transform.position = newPos;

        if (Vector2.Distance(transform.position, moveShootPoints[_selectedShootPoint].position) <= 1f)
        {
            _canMoveShoot = false;
            ChooseNextAction(BombGorilaActions.MoveShoot);
        }
    }

    private IEnumerator FlipX(float t)
    {
        yield return new WaitForSeconds(t);

        if (!_isShooting)
        {
            var dirX = Mathf.Sign(_player.transform.position.x - transform.position.x);

            if (dirX == -1f)
            {
                _spr.flipX = true;
                attackTransform.localPosition = new Vector3(-0.5f, -0.875f, 0f);
                bombSpawnPoint.localPosition = new Vector3(-0.875f, -0.135f, 0f);
            }
            else
            {
                _spr.flipX = false;
                attackTransform.localPosition = new Vector3(1.25f, -0.875f, 0f);
                bombSpawnPoint.localPosition = new Vector3(0.875f, -0.135f, 0f);
            }
        }

        StartCoroutine(FlipX(flipXInterval));
    }

    private IEnumerator StartChangeInterval(float min, float max)
    {
        yield return new WaitForSeconds(Random.Range(min, max + 1));
        _canChange = true;
    }

    private IEnumerator SpawnBomb(float t)
    {
        yield return new WaitForSeconds(t);
        if (_bombCurCount > _bombSelectedCount)
        {
            // Reinicie para a próxima vez
            _bombCurCount = 1;
            _bombSelectedCount = 0;

            // Mude a ação
            ChooseNextAction(BombGorilaActions.Shoot);
        }
        else
        {
            var bomb = Instantiate(bombPrefab, bombSpawnPoint.position, Quaternion.identity);
            _audioManager.PlaySFX("bomba");

            if (_selectedShootPoint == 0)
                bomb.DirX = 1f;
            else
                bomb.DirX = -1f;

            _bombCurCount++;

            StartCoroutine(SpawnBomb(bombInterval));
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
