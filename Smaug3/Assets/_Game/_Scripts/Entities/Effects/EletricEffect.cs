using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EletricEffect : MonoBehaviour
{
    [Header("Enemy References:")]
    [SerializeField] private EnemyCollision enemyColScript;
    [SerializeField] private EnemyBehavior enemyBehaviorScript;
    [SerializeField] private Rigidbody2D enemyRb;

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

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _hitbox = GetComponent<BoxCollider2D>();
        _line = GetComponent<LineRenderer>();
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
            var enemyCol = col.gameObject.GetComponent<EnemyCollision>();
            enemyCol.ChangeCurrentHealth(eletricBanana.Damage);
            enemyCol.ApplyEffect(BananaType.Types.Eletric);

            // Eletric Chain
            _line.enabled = true;
            _eletricChain = col.transform;
        }
    }

    public void Eletrify()
    {
        enemyColScript.gameObject.layer = collisionLayers.IgnoreEletricLayer;
        enemyBehaviorScript.SetMovement(false);
        enemyRb.velocity = Vector2.zero;

        _spr.enabled = true;
        _anim.SetBool("eletrify", true);
        _hitbox.enabled = true;

        StartCoroutine(StopEletrification(eletrificationTime));
    }

    private IEnumerator StopEletrification(float t)
    {
        yield return new WaitForSeconds(t);
        _spr.enabled = false;
        _anim.SetBool("eletrify", false);
        _hitbox.enabled = false;
        enemyBehaviorScript.SetMovement(true);

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
