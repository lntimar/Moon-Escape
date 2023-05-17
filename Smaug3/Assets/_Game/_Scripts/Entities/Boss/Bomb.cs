using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Settings:")] 
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float minHorizontalSpeed;
    [SerializeField] private float maxHorizontalSpeed;
    [SerializeField] private CollisionLayers collisionLayers;
    [SerializeField] private float spawnExplosionTime;

    // Components
    private Rigidbody2D _rb;
    private CircleCollider2D _hitbox;

    // Movement
    [HideInInspector] public float DirX;
    private float _curHorizontalSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hitbox = GetComponent<CircleCollider2D>();

        _curHorizontalSpeed = Random.Range(minHorizontalSpeed, maxHorizontalSpeed);
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_curHorizontalSpeed * DirX, _rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == collisionLayers.WallLayer)
        {
            DirX *= -1f;
        }
        else if (col.gameObject.layer == collisionLayers.GroundLayer)
        {
            DirX = 0f;
            _rb.gravityScale = 0f;
            _rb.velocity = Vector2.zero;
            StartCoroutine(SpawnExplosionInterval(spawnExplosionTime));
            _hitbox.isTrigger = true;
        }
        else if (col.gameObject.layer == collisionLayers.BananaLayer)
        {
            SpawnExplosion();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.BananaLayer)
        {
            SpawnExplosion();
        }
    }

    private void SpawnExplosion()
    {
        Instantiate(explosionPrefab, transform.position + Vector3.up * 0.72f, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator SpawnExplosionInterval(float t)
    {
        yield return new WaitForSeconds(t);
        SpawnExplosion();
    }
}
