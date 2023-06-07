using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackAlienBehaviour : MonoBehaviour
{
    [Header("Height Points:")] 
    [SerializeField] private Transform[] heightPoints;
    [SerializeField] private int[] healthTriggers;
    [SerializeField] private float verifyHeightPointTime;

    [Header("Laser:")]
    [SerializeField] private Transform laserPoint;

    [Header("Raycast:")]
    [SerializeField] private float rayRange;

    [Header("Raycast Ignored Layers:")] 
    [SerializeField] private LayerMask rayIgnoredLayers;

    [Header("Collision Layers:")] 
    [SerializeField] private CollisionLayers collisionLayers;

    // References
    private AudioManager _audioManager;

    // Components
    private Boss _bossScript;
    private BossCollision _bossCollisionScript;
    private SpriteRenderer _spr;
    private Rigidbody2D _rb;
    private DropItem _dropItem;

    // Movement
    private float _horizontalDirection = 1f;
    private int _currentHeightPoint = 0;
    private bool _canFlip = true;

    private bool _canPlayJetpackSFX = true;

    private void Start()
    {
        _bossScript = GetComponent<Boss>();
        _bossCollisionScript = GetComponent<BossCollision>();
        _spr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _dropItem = GetComponent<DropItem>();

        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        StartCoroutine(VerifyHeightPoint(verifyHeightPointTime));
    }

    private void Update()
    {
        ChangeVerticalPosition();
        ChangeLaserHeight();
    }

    private void FixedUpdate()
    {
        ApplyHorizontalMove();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.TriggerFlipLayer && _canFlip)
        {
            _canFlip = false;
            StartCoroutine(ResetCanFlip(2f));
            _horizontalDirection *= -1f;
            
            if (_horizontalDirection == -1f)
            {
                _spr.flipX = true;
                laserPoint.transform.localPosition = new Vector3(-0.155f,
                    laserPoint.transform.localPosition.y, laserPoint.transform.localPosition.z);
            }
            else
            {
                _spr.flipX = false;
                laserPoint.transform.localPosition = new Vector3(-0.887f,
                    laserPoint.transform.localPosition.y, laserPoint.transform.localPosition.z);
            }

            _dropItem.SpawnBonus(false);
        }
    }

    private void ApplyHorizontalMove()
    {
        _rb.velocity = new Vector3(_bossScript.Speed * _horizontalDirection, 0f);
    }

    private void ChangeVerticalPosition()
    {
        // Caso a posição y não for a mesma do heightPoint atual
        if (transform.position.y != heightPoints[_currentHeightPoint].position.y)
        {
            var y = Mathf.Lerp(transform.position.y, heightPoints[_currentHeightPoint].position.y, _bossScript.Speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);

            if (_canPlayJetpackSFX)
            {
                _canPlayJetpackSFX = false;
                _audioManager.PlaySFX("jetpack");
                StartCoroutine(JetpackSFXInterval(0.75f));
            }
        }
    }

    private void ChangeLaserHeight()
    {
        // Crio o Raycast
        RaycastHit2D hit = Physics2D.Raycast(laserPoint.position, Vector2.down, rayRange, ~rayIgnoredLayers);

        // Caso colidir com o chão
        if (hit)
        {
            // Altero a altura do laser
            laserPoint.localScale = new Vector3(laserPoint.localScale.x, hit.distance, laserPoint.localScale.z);

            //Debug.DrawRay(laserPoint.transform.position, Vector3.up * hit.point, Color.green);
        }
    }

    private IEnumerator VerifyHeightPoint(float t)
    {
        yield return new WaitForSeconds(t);
        var curHealth = _bossCollisionScript.GetCurrentHealth();

        // Caso a vida atual for menor que o valor que estamos observando para mudar a altura atual
        if (curHealth < healthTriggers[_currentHeightPoint])
        {
            // Vá para o próximo Height Point
            _currentHeightPoint++;
        }

        // Chame a Coroutine denovo caso ainda não estivermos no último heightPoint
        if (_currentHeightPoint < heightPoints.Length - 1) StartCoroutine(VerifyHeightPoint(t));
    }

    private IEnumerator ResetCanFlip(float t)
    {
        yield return new WaitForSeconds(t);
        _canFlip = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        laserPoint.gameObject.SetActive(false);
    }

    private IEnumerator JetpackSFXInterval(float t)
    {
        yield return new WaitForSeconds(t);
        _canPlayJetpackSFX = true;
    }

    private void OnDestroy()
    {
        _audioManager.PlaySFX("colecionaveis");
    }
}
