using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    [Header("Health:")] 
    [SerializeField] private int maxHealth; // Provavelmente vai ser static
    [SerializeField] private int inspectorCurrentHealth;
    private static int currentHealth; // Provavelmente vai ser static
    [SerializeField] private SpriteRenderer playerDeathSpr;
    [SerializeField] private Animator playerDeathAnim;

    [Header("Invencibility:")] 
    [SerializeField] private float invencibilityTime;

    [Header("Collision Layers:")] 
    [SerializeField] CollisionLayers collisionLayers;

    [Header("Knockback:")] 
    [SerializeField] private float knockbackForce;

    [SerializeField] private float knobackTime;

    [Header("Eletric Effect")] 
    [SerializeField] private PlayerEletricEffect eletricEffect;

    [Header("Restart Scene:")] [SerializeField]
    private float restartSceneTime;

    // References
    private FadeVFX _fadeIn;
    private PlayerHealthBar _playerHealthBar;

    // Components
    private BlinkSpriteVFX _blinkVFX;
    private SpriteRenderer _spr;
    private Rigidbody2D _rb;
    private Animator _anim;
    private PlayerIceEffect _iceEffect;

    // Stop Animator
    private float _animDefaultSpeed;

    public static bool BossBattle;

    private void Start()
    {
        _blinkVFX = GetComponent<BlinkSpriteVFX>();
        _spr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _iceEffect = GetComponent<PlayerIceEffect>();

        _animDefaultSpeed = _anim.speed;
        _fadeIn = GameObject.FindGameObjectWithTag("Fade In").GetComponent<FadeVFX>();

        _playerHealthBar = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<PlayerHealthBar>();
        
        if (currentHealth == 0)
        {
            ChangeCurrentHealth(maxHealth);
            var playerShootScript = GetComponent<PlayerShoot>();
            playerShootScript.ChangeCurrentEnergy(playerShootScript.maxEnergy);
        }
    }

    private void Update()
    {
        inspectorCurrentHealth = currentHealth;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Ignorar Colisão caso o Player estiver fazendo um Dash / Carregando
        if (PlayerStateMachine.StateManager.CompareState(PlayerStateMachine.PlayerStates.Dashing) 
            || PlayerStateMachine.StateManager.CompareState(PlayerStateMachine.PlayerStates.Loading)) return;

        if (col.gameObject.layer == collisionLayers.EnemyLayer || col.gameObject.layer == collisionLayers.BossLayer)
        {
            // Aplicando Dano
            if (col.gameObject.layer == collisionLayers.EnemyLayer)
            {
                ChangeCurrentHealth(-col.gameObject.GetComponent<Enemy>().Damage);

                if (col.gameObject.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour behaviourScript))
                {
                    if (!behaviourScript.isRange) behaviourScript.Anim.SetTrigger("Attacking");
                }
            }
            else
            {
                ChangeCurrentHealth(-col.gameObject.GetComponent<Boss>().Damage);
            }

            // Caso for o dano elétrico do Gorila Elétrico
            if (col.gameObject.name.Contains("EletricAttack"))
            {
                if (col.gameObject.tag == "Enemy Attack")
                {
                    if (_spr.flipX) ApplyKnockback(Vector2.right);
                    else ApplyKnockback(Vector2.left);
                }

                eletricEffect.ActivateEletrify();
            }// Caso for o dano gélico do Gorila de Gelo
            else if (col.gameObject.name.Contains("IceTrap") || col.gameObject.name.Contains("IceAttack"))
            {
                if (col.gameObject.tag == "Enemy Attack")
                {
                    if (_spr.flipX) ApplyKnockback(Vector2.right);
                    else ApplyKnockback(Vector2.left);
                }

                // Caso não estiver congelado, congele o Player
                if (!_iceEffect.IsFreeze) _iceEffect.Freeze();
            }
            else // Se não, é qualquer outro tipo de inimigo / dano
            {
                // Aplicando Knockback
                if (col.gameObject.GetComponent<SpriteRenderer>().flipX) ApplyKnockback(Vector2.left);
                else ApplyKnockback(Vector2.right);

                // Aplicando Invencibilidade temporária
                StartCoroutine(SetInvencibilityInterval(invencibilityTime));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (PlayerStateMachine.StateManager.CompareState(PlayerStateMachine.PlayerStates.Dashing)
            || PlayerStateMachine.StateManager.CompareState(PlayerStateMachine.PlayerStates.Loading)) return;

        if (col.gameObject.layer == collisionLayers.EnemyLayer || col.gameObject.layer == collisionLayers.BossLayer)
        {
            // Aplicando Dano
            if (col.gameObject.layer == collisionLayers.EnemyLayer)
            {
                ChangeCurrentHealth(-col.gameObject.GetComponent<Enemy>().Damage);
            }
            else
            {
                ChangeCurrentHealth(-col.gameObject.GetComponent<Boss>().Damage);
            }

            // Caso for o dano elétrico do Gorila Elétrico
            if (col.gameObject.name.Contains("EletricAttack") || col.gameObject.name.Contains("Eletric Companion Orb"))
            {
                if (col.gameObject.tag == "Enemy Attack")
                {
                    if (_spr.flipX) ApplyKnockback(Vector2.right);
                    else ApplyKnockback(Vector2.left);
                }
                eletricEffect.ActivateEletrify();
            }// Caso for o dano gélico do Gorila de Gelo
            else if (col.gameObject.name.Contains("IceTrap") || col.gameObject.name.Contains("IceAttack") || col.gameObject.name.Contains("Ice Companion Orb"))
            {
                if (col.gameObject.tag == "Enemy Attack")
                {
                    if (_spr.flipX) ApplyKnockback(Vector2.right);
                    else ApplyKnockback(Vector2.left);
                }

                // Caso não estiver congelado, congele o Player
                if (!_iceEffect.IsFreeze) _iceEffect.Freeze();
            }
            else // Se não, é qualquer outro tipo de inimigo / dano
            {
                // Aplicando Knockback
                if (!col.gameObject.name.Contains("EletricTrap"))
                {
                    if (col.gameObject.tag != "Enemy Attack" && !col.gameObject.name.Contains("Bomb Explosion"))
                    {
                        if (col.gameObject.GetComponent<SpriteRenderer>().flipX) ApplyKnockback(Vector2.left);
                        else ApplyKnockback(Vector2.right);
                    }
                    else
                    {
                        if (_spr.flipX) ApplyKnockback(Vector2.right);
                        else ApplyKnockback(Vector2.left);
                    }

                    // Aplicando Invencibilidade temporária
                    StartCoroutine(SetInvencibilityInterval(invencibilityTime));
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.EnemyLayer && col.gameObject.name.Contains("EletricTrap"))
        {
            _rb.velocity = Vector2.zero;
            eletricEffect.ActivateEletrify(false);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.EnemyLayer && col.gameObject.name.Contains("EletricTrap"))
        {
            eletricEffect.StopEletrification();
        }
    }

    public void ChangeCurrentHealth(int points)
    {
        currentHealth = Mathf.Clamp(currentHealth + points, 0, maxHealth);
        _playerHealthBar.SetHealthBar(currentHealth);

        if (currentHealth == 0 && !playerDeathSpr.enabled)
        {
            PlayerStateMachine.StateManager.SetState(PlayerStateMachine.PlayerStates.Dead);
            playerDeathSpr.enabled = true;

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerShoot>().enabled = false;

            if (_spr.flipX) playerDeathAnim.SetTrigger("deadLeft");
            else playerDeathAnim.SetTrigger("deadRight");

            _rb.velocity = Vector2.zero;
            _blinkVFX.enabled = false;
            _spr.enabled = false;
            _anim.enabled = false;

            var alpha = _spr.color;
            alpha.a *= 0.75f;
            _spr.color = alpha;

            _fadeIn.enabled = true;

            StartCoroutine(RestartScene(restartSceneTime));
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    } 

    private IEnumerator SetInvencibilityInterval(float t)
    {
        _blinkVFX.SetBlink();
        // Desabilito a colisão entre o player e os inimigos
        Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.EnemyLayer, true);
        yield return new WaitForSeconds(t);
        // Habilito novamente a colisão entre o player e os inimigos
        Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.EnemyLayer, false);
    }

    public void ApplyKnockback(Vector2 dir)
    {
        _anim.speed = 0;
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        _rb.AddForce(dir * knockbackForce * Time.deltaTime, ForceMode2D.Impulse);
        PlayerStateMachine.StateManager.SetState(PlayerStateMachine.PlayerStates.Damaged);
        StartCoroutine(StopKnockback(knobackTime));
    }

    private IEnumerator StopKnockback(float t)
    {
        yield return new WaitForSeconds(t);
        PlayerStateMachine.StateManager.SetState(PlayerStateMachine.PlayerStates.Idle);
        _anim.speed = _animDefaultSpeed;
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    private IEnumerator RestartScene(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
        if (BossBattle) SceneManager.LoadScene(BackTrack.PreviousScene);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
