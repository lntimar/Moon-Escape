using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletMovement : MonoBehaviour
{
    [Header("Collision Layers:")]
    [SerializeField] private CollisionLayers collisionLayers;

    public float moveDir;
    public float lifeTime = 5;

    // Components
    private Rigidbody2D _rb;
    private Enemy _enemyScript;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyScript = GetComponent<Enemy>();

        Destroy(gameObject, lifeTime);

        if (moveDir == -1f) GetComponent<SpriteRenderer>().flipX = true;

        //spawna uma bala do bullet spawn position
        //qnd spawna da play no shooting, mexer isso no animator
        //bala segue reto pra direção que o enemy ta olhando
        //se encosta no player tira vida dele
        //se encosta no player ou em algum tile ou passa determinado tempo a bala é destruida
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_enemyScript.Speed * moveDir, 0f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.GroundLayer)
        {
            Destroy(gameObject);
        }
        else if (col.gameObject.layer == collisionLayers.WallLayer)
        {
            Destroy(gameObject);
        }
        else if (col.gameObject.layer == collisionLayers.PlayerLayer)
        {
            Destroy(gameObject);
        }
    }
}
