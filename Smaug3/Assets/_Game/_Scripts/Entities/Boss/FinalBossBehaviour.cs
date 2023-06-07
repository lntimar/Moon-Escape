using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossBehaviour : MonoBehaviour
{
    [Header("Movement:")]
    [SerializeField] private Transform[] movePoints;

    [Header("Laser:")] 
    [SerializeField] private float minLaserTime;
    [SerializeField] private float maxLaserTime;
    [SerializeField] private GameObject laser;

    [Header("Floating Movement:")]
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float changeVerticalDirTime;

    [Header("Change Position:")]
    [SerializeField] private float minWaitingChangePosition;
    [SerializeField] private float maxWaitingChangePosition;

    [Header("Collision Layers:")]
    [SerializeField] private CollisionLayers collisionLayers;

    [Header("Effects:")]
    [SerializeField] private Transform bombEffectTransform;

    [Header("Companions:")] 
    public GameObject[] Companions;

    // References
    private AudioManager _audioManager;

    // Components
    private Boss _bossScript;
    private BossCollision _bossCollisionScript;
    private DropItem _dropItem;

    // Movement
    private bool _canMove = false;
    private int _selectedPoint;

    // Floating Movement
    private bool _canFloatMove = false;
    private float _curVerticalDir = -1f;

    private void Start()
    {
        _bossScript = GetComponent<Boss>();
        _bossCollisionScript = GetComponent<BossCollision>();
        _dropItem = GetComponent<DropItem>();

        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        _audioManager.PlayMusic("final boss");
    }

    private void Update()
    {
        if (_canMove)
        {
            Move();
        }

        // Caso zerar a vida
        if (_bossCollisionScript.GetCurrentHealth() == 0f)
        {
            // Destrua os Ajudantes
            for (int i = 0; i < Companions.Length; i++)
            {
                Destroy(Companions[i]);
            }
        }

        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 6f)
        {
            laser.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (_canFloatMove) transform.position += Vector3.up * _curVerticalDir * verticalSpeed * Time.fixedDeltaTime;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void EnableMove()
    {
        StopAllCoroutines();
        _canMove = true;
        _canFloatMove = false;
        StartCoroutine(EnableLaser(Random.Range(minLaserTime, maxLaserTime)));
        
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
            _canFloatMove = true;
            StartCoroutine(ChangePositionInterval(Random.Range(minWaitingChangePosition, maxWaitingChangePosition)));
            StartCoroutine(ChangeVerticalDirection(changeVerticalDirTime));
        }
    }

    private IEnumerator ChangePositionInterval(float t)
    {
        yield return new WaitForSeconds(t);
        EnableMove();
    }

    private IEnumerator ChangeVerticalDirection(float t)
    {
        yield return new WaitForSeconds(t);
        _curVerticalDir *= -1f;

        StartCoroutine(ChangeVerticalDirection(changeVerticalDirTime));
    }

    public void EnableBoss()
    {
        _canFloatMove = true;
        StartCoroutine(ChangePositionInterval(Random.Range(minWaitingChangePosition, maxWaitingChangePosition)));
        StartCoroutine(ChangeVerticalDirection(changeVerticalDirTime));

        for (int i = 0; i < Companions.Length; i++)
        {
            Companions[i].SetActive(true);
        }
    }

    private IEnumerator EnableLaser(float t)
    {
        yield return new WaitForSeconds(t);
        laser.SetActive(true);
        StartCoroutine(EnableLaser(Random.Range(minLaserTime, maxLaserTime)));
    }
}
