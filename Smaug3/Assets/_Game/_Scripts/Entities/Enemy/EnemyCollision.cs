using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [Header("Life:")]
    [SerializeField] private int currentHealth;
    
    [Header("Knockback:")]
    [SerializeField] private float knockbackForce;
    
    [Header("Effects:")]
    [SerializeField] private GameObject[] effects;

    [Header("Collision Layers:")]
    [SerializeField] CollisionLayers collisionLayers;

    [Header("Items:")] 
    [SerializeField] private GameObject itemLifePrefab;
    [SerializeField] private GameObject itemEnergyPrefab;
    [SerializeField] private int minBonusHealth;
    [SerializeField] private int minBonusEnergy;

    // Components
    private CapsuleCollider2D _hitbox;
    private SpriteRenderer _spr;
    private Animator _anim;
    private Enemy _enemy;
    private EnemyBehaviour _enemyBehaviour;
    private BlinkSpriteVFX _blink;
    private Rigidbody2D _rb;

    private bool _blinkAttack = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _spr = GetComponent<SpriteRenderer>();
        _hitbox = GetComponent<CapsuleCollider2D>();
        _enemy = GetComponent<Enemy>();
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
        _blink = GetComponent<BlinkSpriteVFX>();
        _rb = GetComponent<Rigidbody2D>();

        ChangeCurrentHealth(_enemy.MaxHealth);
    }

    private void Update()
    {
        if (_blinkAttack)
        {
            _blink.SetBlink();
            _blinkAttack = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.AttackLayer)
        {
            ChangeCurrentHealth(-col.gameObject.transform.parent.gameObject.GetComponent<InitialPlayerAttack>().damage);
            var playerSpr = col.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>();
            
            float knockbackDir;

            if (playerSpr.flipX) knockbackDir = -1f;
            else knockbackDir = 1f;

            ApplyKnockback(Vector2.right * knockbackDir, 2f);

            _blinkAttack = true;
        }
    }

    public void ChangeCurrentHealth(int points)
    {
        currentHealth = Mathf.Clamp(currentHealth + points, 0, _enemy.MaxHealth);

        if (currentHealth == 0)
        {
            // Inimigo Morre
            StopAllCoroutines();
            Destroy(_enemyBehaviour);
            Destroy(_hitbox);
            Destroy(_rb);
            _spr.enabled = true;
            _spr.flipX = false;
            _blink.enabled = false;

            _anim.Play("Death Animation");
        }
    }

    public void KillEnemy()
    {
        SpawnBonus(GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerShoot>(out PlayerShoot playerShoot));
        Destroy(gameObject, 0.5f);
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ApplyEffect(BananaType.Types effect)
    {
        switch (effect)
        {
            case BananaType.Types.Default:
                _blink.SetBlink();
                break;

            case BananaType.Types.Ice:
                effects[0].GetComponent<EnemyIceEffect>().Freeze();
                break;

            case BananaType.Types.Bomb:
                effects[1].GetComponent<EnemyBombEffect>().Explode();
                _blink.SetBlink();
                break;

            case BananaType.Types.Eletric:
                effects[2].GetComponent<EnemyEletricEffect>().Eletrify();
                _blink.SetBlink();
                break;
        }
    }

    public void ApplyKnockback(Vector2 direction, float multiplier)
    {
        StopAllCoroutines();
        _enemyBehaviour.enabled = false;
        _rb.AddForce(direction * knockbackForce * multiplier, ForceMode2D.Impulse);
        StartCoroutine(ResetMovement(0.1f));
    }

    private IEnumerator ResetMovement(float t)
    {
        yield return new WaitForSeconds(t);
        if (currentHealth != 0)
            _enemyBehaviour.enabled = true;
    }

    private void SpawnBonus(bool isMecha)
    {
        if (isMecha)
        {
            int currentEnergy = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>().GetCurrentEnergy();
            int currentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollision>().GetCurrentHealth();

            int option = 0;

            if (currentHealth <= minBonusHealth && currentEnergy > minBonusEnergy)
            {
                if (Random.Range(0, 100) < 50)
                {
                    option = 1;
                }
            }
            else if (currentEnergy <= minBonusEnergy && currentHealth > minBonusHealth)
            {
                if (Random.Range(0, 100) < 50)
                {
                    option = 2;
                }
            }
            else if (currentHealth <= minBonusHealth && currentEnergy <= minBonusEnergy)
            {
                if (Random.Range(0, 100) < 50)
                {
                    if (Random.Range(0, 100) < 50)
                    {
                        option = 1;
                    }
                }
                else
                {
                    if (Random.Range(0, 100) < 50)
                    {
                        option = 2;
                    }
                }
            }
            else
            {
                if (Random.Range(0, 100) < 50)
                {
                    if (Random.Range(0, 100) < 25)
                    {
                        option = 1;
                    }
                }
                else
                {
                    if (Random.Range(0, 100) < 25)
                    {
                        option = 2;
                    }
                }
            }

            switch (option)
            {
                case 1:
                    Instantiate(itemLifePrefab, transform.position - new Vector3(0f, 0.35f, 0f), Quaternion.identity);
                    break;

                case 2:
                    Instantiate(itemEnergyPrefab, transform.position - new Vector3(0f, 0.35f, 0f), Quaternion.identity);
                    break;
            }
        }
        else
        {
            int currentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<InitialPlayerCollision>().GetCurrentHealth();

            if (currentHealth <= minBonusHealth)
            {
                if (Random.Range(0, 100) < 50)
                    Instantiate(itemLifePrefab, transform.position - new Vector3(0f, 0.35f, 0f), Quaternion.identity);
            }
            else
            {
                if (Random.Range(0, 100) < 25)
                    Instantiate(itemLifePrefab, transform.position - new Vector3(0f, 0.35f, 0f), Quaternion.identity);
            }
        }
    }
}
