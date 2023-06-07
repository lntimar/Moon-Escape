using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorilaEletricBehaviour : MonoBehaviour
{
    [Header("Movement:")]
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float flipXInterval;

    [Header("Start Animation:")]
    [SerializeField] private float minWaitingInterval;
    [SerializeField] private float maxWaitingInterval;

    [Header("Attack:")] 
    [SerializeField] private Transform eletricAttackTransform;
    [SerializeField] private EletricTrapSet[] eletricTrapsSets;

    [Header("Collision Layers:")]
    [SerializeField] private CollisionLayers collisionLayers;

    [Header("Effects:")] 
    [SerializeField] private Transform bombEffectTransform;

    // References
    private GameObject _player;
    private AudioManager _audioManager;

    // Components
    private SpriteRenderer _spr;
    private Animator _anim;
    private Boss _bossScript;
    private DropItem _dropItem;

    // Movement
    private bool _canMove = false;
    private int _selectedPoint;

    // Start Anim
    private bool _isWaiting = false;
    private bool _canChange = false;

    // Eletric Trap
    private int _ignoredIndex = -1;

    public enum EletricGorilaActions
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
        _dropItem = GetComponent<DropItem>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        StartCoroutine(FlipX(flipXInterval));

        _audioManager.PlayMusic("gorila eletric");
    }

    private void Update()
    {
        if (_canMove)
        {
            Move();
        }

        if (Vector2.Distance(transform.position, _player.transform.position) <= 6f)
        {
            if (Random.Range(0, 100) < 5)
            {
                _anim.Play("Gorila Eletric Attack Animation");
                _isWaiting = false;
                _canChange = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<IgnoredEletricTrap>(out IgnoredEletricTrap ignoredEletricTrap))
        {
            _ignoredIndex = ignoredEletricTrap.IgnoredIndex;
        }
    }

    public void ChooseNextAction(EletricGorilaActions currentAction)
    {
        if (currentAction == EletricGorilaActions.Start)
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
                    _anim.Play("Gorila Eletric Move Animation");
                    EnableMove();
                }
                else
                {
                    _anim.Play("Gorila Eletric Attack Animation");
                }

                // Reinicie para a próxima vez
                _canChange = false;
                _isWaiting = false;
            }
        }
        else if (currentAction == EletricGorilaActions.Move)
        {
            _anim.Play("Gorila Eletric Attack Animation");
        }
        else // Attack
        {
            _anim.Play("Gorila Eletric Start Animation");
        }
    }

    public void EnableEletricTraps()
    {
        for (int i = 0; i < eletricTrapsSets.Length; i++)
        {
            if (i != _ignoredIndex)
            {
                for (int j = 0; j < eletricTrapsSets[i].EletricTraps.Length; j++)
                {
                    eletricTrapsSets[i].EletricTraps[j].SetActive(true);
                }
            }
        }
    }

    public void DisableEletricTraps()
    {
        for (int i = 0; i < eletricTrapsSets.Length; i++)
        {
            if (i != _ignoredIndex)
            {
                for (int j = 0; j < eletricTrapsSets[i].EletricTraps.Length; j++)
                {
                    eletricTrapsSets[i].EletricTraps[j].SetActive(false);
                }
            }
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

        _dropItem.SpawnBonus(true);

        _selectedPoint = newPoint;
    }

    private void Move()
    {
        var newPos = Vector3.Lerp(transform.position, movePoints[_selectedPoint].position, _bossScript.Speed * Time.deltaTime);
        transform.position = newPos;

        if (Vector2.Distance(transform.position, movePoints[_selectedPoint].position) <= 1f)
        {
            _canMove = false;
            ChooseNextAction(EletricGorilaActions.Move);
        }
    }

    private IEnumerator FlipX(float t)
    {
        yield return new WaitForSeconds(t);
        var dirX = Mathf.Sign(_player.transform.position.x - transform.position.x);

        if (dirX == -1f)
        {
            _spr.flipX = true;
            eletricAttackTransform.localPosition = new Vector3(-0.5f, -0.875f, 0f);
            bombEffectTransform.localPosition = new Vector3(0.25f, -0.125f, 0);
        }
        else
        {
            _spr.flipX = false;
            eletricAttackTransform.localPosition = new Vector3(1.25f, -0.875f, 0f);
            bombEffectTransform.localPosition = new Vector3(-0.25f, -0.125f, 0);
        }

        StartCoroutine(FlipX(flipXInterval));
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
