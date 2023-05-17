using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBombEffect : MonoBehaviour
{   
    [Header("Enemy References:")]
    [SerializeField] private EnemyCollision enemyColScript;
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
            if (col.gameObject.TryGetComponent<EnemyCollision>(out EnemyCollision enemyCol))
            {
                enemyCol.ChangeCurrentHealth(bombBanana.Damage);
                enemyCol.ApplyEffect(BananaType.Types.Bomb);
                enemyCol.ApplyKnockback(enemyRb.velocity.normalized * 1.55f, 1f);
            }
        }
    }

    public void Explode()
    {
        enemyColScript.gameObject.layer = collisionLayers.IgnoreExplosionLayer;
        _anim.SetTrigger("explode");
        if (Random.Range(0, 100) <= 50) _anim.SetInteger("rand", 1);
        else _anim.SetInteger("rand", -1);
        _hitbox.enabled = true;

        StartCoroutine(StopExplosion(explosionTime));
        StartCoroutine(SetCooldown(cooldownTime));
    }

    private IEnumerator StopExplosion(float t)
    {
        yield return new WaitForSeconds(t);
        _hitbox.enabled = false;
        _anim.SetInteger("rand", 0);
    }

    private IEnumerator SetCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        enemyColScript.gameObject.layer = collisionLayers.EnemyLayer;
    }
}
