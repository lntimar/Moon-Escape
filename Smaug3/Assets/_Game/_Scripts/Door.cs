using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite openSprite;
    [SerializeField] private float openSpeed;
    [SerializeField] private CollisionLayers collisionLayers;

    // References
    private AudioManager _audioManager;

    // Components
    private SpriteRenderer _spr;
    private BoxCollider2D _hitbox;
    private Rigidbody2D _rb;

    private bool _canMove = false;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
        _hitbox = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();

        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void FixedUpdate()
    {
        if (_canMove) transform.position += Vector3.up * openSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == collisionLayers.DoorTriggerLayer)
        {
            _canMove = false;
        }
    }

    public void Open()
    {
        _audioManager.PlaySFX("porta");
        _canMove = true;
        _spr.sprite = openSprite;
    }
}
