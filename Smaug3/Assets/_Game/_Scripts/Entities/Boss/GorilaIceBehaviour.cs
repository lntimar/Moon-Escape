using System.Collections;
using UnityEngine;

public class GorilaIceBehaviour : MonoBehaviour
{
    [Header("Movement:")]
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float flipXInterval;

    [Header("Start Animation:")] 
    [SerializeField] private float minWaitingInterval;
    [SerializeField] private float maxWaitingInterval;

    [Header("Attack:")]
    [SerializeField] private IceTrapSpawner[] iceTrapSpawners;
    [SerializeField] private float setIceTrapsInterval;
    [SerializeField] private float minDistanceIceTraps;
    [SerializeField] private Transform iceAttackTransform;

    [Header("Collision Layers:")]
    [SerializeField] private CollisionLayers collisionLayers;

    // References
    private GameObject _player;

    // Components
    private SpriteRenderer _spr;
    private Animator _anim;
    private Boss _bossScript;
    private BossCollision _bossCollisionScript;

    // Movement
    private bool _canMove = false;
    private int _selectedPoint;

    // Set Ice Traps
    private int _curIceTrapSpawnerIndex = 0;

    // Start Anim
    private bool _isWaiting = false;
    private bool _canChange = false;

    public enum IceGorilaActions
    {
        Start,
        Move,
        Attack
    }

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _bossScript = GetComponent<Boss>();
        _bossCollisionScript = GetComponent<BossCollision>();

        _player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(FlipX(flipXInterval));
    }

    private void Update()
    {
        if (_canMove)
        {
            Move();
        }

        if (Vector2.Distance(transform.position, _player.transform.position) <= 6f)
        {
            _anim.Play("Gorila Ice Attack Animation");
            _isWaiting = false;
            _canChange = false;
        }
    }

    public void ChooseNextAction(IceGorilaActions currentAction)
    {
        if (currentAction == IceGorilaActions.Start)
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
                if (Random.Range(0, 100) < 50)
                {
                    _anim.Play("Gorila Ice Move Animation");
                    EnableMove();
                }
                else
                {
                    _anim.Play("Gorila Ice Attack Animation");
                }

                // Reinicie para a próxima vez
                _canChange = false;
                _isWaiting = false;
            }
        }
        else if (currentAction == IceGorilaActions.Move)
        {
            _anim.Play("Gorila Ice Attack Animation");
        }
        else // Attack
        {
            _anim.Play("Gorila Ice Start Animation");
        }
    }

    public void EnableIceTraps()
    {
        if (iceTrapSpawners[0].CanDrop && Vector2.Distance(transform.position, _player.transform.position) >= minDistanceIceTraps)
        {
            // Ativar as estalactites
            StartCoroutine(SetIceTraps(setIceTrapsInterval));
        }
    }

    public void EnableMove()
    {
        _canMove = true;

        int newPoint = Random.Range(0, movePoints.Length);
        while (_selectedPoint == newPoint)
        {
            newPoint = Random.Range(0, movePoints.Length);
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
            ChooseNextAction(IceGorilaActions.Move);
        }
    }

    private IEnumerator FlipX(float t)
    {
        yield return new WaitForSeconds(t);
        var dirX = Mathf.Sign(_player.transform.position.x - transform.position.x);

        if (dirX == -1f)
        {
            _spr.flipX = true;
            iceAttackTransform.localPosition = new Vector3(-0.5f, -0.875f, 0f);
        }
        else
        {
            _spr.flipX = false;
            iceAttackTransform.localPosition = new Vector3(1.25f, -0.875f, 0f);
        }

        StartCoroutine(FlipX(flipXInterval));
    }

    private IEnumerator SetIceTraps(float t)
    {
        yield return new WaitForSeconds(t);
        // Drope a estalactite
        iceTrapSpawners[_curIceTrapSpawnerIndex].DropIceTrap();
        iceTrapSpawners[_curIceTrapSpawnerIndex].CanDrop = false;
        
        if (_curIceTrapSpawnerIndex != iceTrapSpawners.Length - 1)
        {
            // Avance para a próxima estalactite
            _curIceTrapSpawnerIndex++;
            StartCoroutine(SetIceTraps(setIceTrapsInterval));
        }
        else
        {
            // Reinicie a contagem para podermos usar novamente na próxima vez
            _curIceTrapSpawnerIndex = 0;
        }
    }

    private IEnumerator StartChangeInterval(float min, float max)
    {
        yield return new WaitForSeconds(Random.Range(min, max + 1));
        _canChange = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
