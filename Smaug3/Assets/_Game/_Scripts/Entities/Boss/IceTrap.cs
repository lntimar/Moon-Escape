using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTrap : MonoBehaviour
{
    [Header("Collision Layers:")] 
    [SerializeField] private CollisionLayers collisionLayers;

    [Header("Move Effect")]
    [SerializeField] private Color effectColor;
    [SerializeField] private GameObject iceTrapEffectPrefab;

    // References
    private AudioManager _audioManager;

    // Components
    private Rigidbody2D _rb;
    private Animator _anim;
    private Enemy _enemyScript;
    private BoxCollider2D _hitbox;
    private DropItem _dropItem;

    // Movement
    private bool _canMove = true;

    // Drop Item
    private bool _colBanana = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _enemyScript = GetComponent<Enemy>();
        _hitbox = GetComponent<BoxCollider2D>();
        _dropItem = GetComponent<DropItem>();
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();


        StartCoroutine(ApplyEffect(0.02f));
    }

    private void FixedUpdate()
    {
        if (_canMove) _rb.velocity = Vector2.down * _enemyScript.Speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.BananaLayer)
        {
            _colBanana = true;
        }

        _audioManager.PlaySFX("estalactite estilhacando");
        _anim.Play("IceTrap Destroy Animation");
        _hitbox.enabled = false;

        _canMove = false;
        _rb.velocity = Vector2.zero;
    }

    // Chame no último frame da animação de destruição
    public void DestroyIceTrap()
    {
        if (_colBanana) _dropItem.SpawnBonus(false);

        Destroy(gameObject);
    }

    private IEnumerator ApplyEffect(float t)
    {
        yield return new WaitForSeconds(t);
        var effect = Instantiate(iceTrapEffectPrefab, transform.position, Quaternion.identity);
        var effectSpr = effect.GetComponent<SpriteRenderer>();
        effectSpr.sprite = GetComponent<SpriteRenderer>().sprite;
        effectSpr.color = effectColor;
        StartCoroutine(ApplyEffect(0.02f));
    }
}
