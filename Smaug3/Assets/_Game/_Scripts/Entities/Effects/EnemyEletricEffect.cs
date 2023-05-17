using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEletricEffect : MonoBehaviour
{
    [Header("Enemy References:")]
    [SerializeField] private EnemyCollision enemyColScript;
    [SerializeField] private EnemyBehaviour enemyBehaviourScript;
    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] private Animator enemyAnim;

    [Header("Eletric Banana:")] 
    [SerializeField] private Banana eletricBanana;

    [Header("Collision Layers:")] 
    [SerializeField] private CollisionLayers collisionLayers;

    [Header("Stop Eletrification:")] 
    [SerializeField] private float eletrificationTime;
    [SerializeField] private float cooldownTime;

    // Components
    private SpriteRenderer _spr;
    private Animator _anim;
    private BoxCollider2D _hitbox;
    private LineRenderer _line;

    // Eletric Chain
    private Transform _eletricChain;

    private float _enemyAnimDefaultSpeed;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _hitbox = GetComponent<BoxCollider2D>();
        _line = GetComponent<LineRenderer>();

        _enemyAnimDefaultSpeed = enemyAnim.speed;
    }

    private void Update()
    {
        if (_eletricChain != null)
        {
            _line.SetPosition(0, transform.position + Vector3.up * Random.Range(-0.75f, 0.75f));
            _line.SetPosition(1, _eletricChain.position + Vector3.up * Random.Range(-0.75f, 0.75f));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.EnemyLayer)
        {
            if (col.gameObject.TryGetComponent<EnemyCollision>(out EnemyCollision enemyCol))
            {
                enemyCol.ChangeCurrentHealth(eletricBanana.Damage);
                enemyCol.ApplyEffect(BananaType.Types.Eletric);

                // Eletric Chain
                _line.enabled = true;
                _eletricChain = col.transform;
            }
        }
    }

    public void Eletrify()
    {
        enemyColScript.gameObject.layer = collisionLayers.IgnoreEletricLayer;
        enemyBehaviourScript.enabled = false;
        enemyRb.velocity = new Vector2(0f, enemyRb.velocity.y);
        enemyAnim.Play("Enemy_Idle");
        enemyAnim.speed = 0;

        _spr.enabled = true;
        _anim.SetBool("eletrify", true);
        _hitbox.enabled = true;

        StartCoroutine(StopEletrification(eletrificationTime));
    }

    private IEnumerator StopEletrification(float t)
    {
        enemyRb.velocity = new Vector2(0f, enemyRb.velocity.y);
        yield return new WaitForSeconds(t);
        _spr.enabled = false;
        _anim.SetBool("eletrify", false);
        _hitbox.enabled = false;
        enemyBehaviourScript.enabled = true;
        enemyBehaviourScript.StartCoroutine(enemyBehaviourScript.EnemyWaits());
        enemyAnim.speed = _enemyAnimDefaultSpeed;

        _line.enabled = false;
        _eletricChain = null;

        StartCoroutine(SetCooldown(cooldownTime));
    }

    private IEnumerator SetCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        enemyColScript.gameObject.layer = collisionLayers.EnemyLayer;
    }
}
