using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : MonoBehaviour
{   
    [Header("Enemy References:")]
    [SerializeField] private EnemyCollision enemyColScript;
    [SerializeField] private EnemyBehavior enemyBehaviorScript;
    [SerializeField] private Rigidbody2D enemyRb;

    [Header("Bomb Banana:")] 
    [SerializeField] private Banana bombBanana;

    [Header("Collision Layers:")]
    [SerializeField] CollisionLayers collisionLayers;

    [Header("Stop Explosion:")] 
    [SerializeField] private float explosionTime;
    [SerializeField] private float cooldownTime;

    // Components
    private Animator _anim;
    private BoxCollider2D _hitbox;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _hitbox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.EnemyLayer)
        {
            var enemyCol = col.gameObject.GetComponent<EnemyCollision>();
            enemyCol.ChangeCurrentHealth(bombBanana.Damage);
            enemyCol.ApplyEffect(BananaType.Types.Bomb);
            enemyCol.ApplyKnockback(enemyRb.velocity.normalized * 1.55f);
        }
    }

    public void Explode()
    {
        enemyColScript.gameObject.layer = collisionLayers.IgnoreExplosionLayer;
        _anim.SetTrigger("explode");
        _hitbox.enabled = true;

        StartCoroutine(StopExplosion(explosionTime));
        StartCoroutine(SetCooldown(cooldownTime));
    }

    private IEnumerator StopExplosion(float t)
    {
        yield return new WaitForSeconds(t);
        _hitbox.enabled = false;
    }

    private IEnumerator SetCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        enemyColScript.gameObject.layer = collisionLayers.EnemyLayer;
    }
}
