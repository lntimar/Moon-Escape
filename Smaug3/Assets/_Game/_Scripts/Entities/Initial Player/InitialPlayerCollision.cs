using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InitialPlayerCollision : MonoBehaviour
{
    [Header("Health:")]
    [SerializeField] private int maxHealth;
    public static int currentHealth;

    [Header("Invencibility")] 
    [SerializeField] private float invencibilityTime;

    [Header("Collision Layers:")]
    [SerializeField] CollisionLayers collisionLayers;

    [Header("Knockback:")]
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knobackTime;

    [Header("Restart Scene:")] 
    [SerializeField] private float restartSceneTime;

    //Canva game over
    [SerializeField] private GameObject gameOverCanva;

    // References
    private FadeVFX _fadeIn;
    private PlayerHealthBar _playerHealthBar;
    private AudioManager _audioManager;

    // Components
    private BlinkSpriteVFX _blinkVFX;
    private InitialPlayerAttack _playerAttack;
    private SpriteRenderer _spr;
    private Animator _anim;
    private Rigidbody2D _rb;

    private float _animDefaultSpeed;

    private void Start()
    {
        _playerHealthBar = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<PlayerHealthBar>();
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();


        if (currentHealth == 0)
        {
            ChangeCurrentHealth(maxHealth);
        }

        _blinkVFX = GetComponent<BlinkSpriteVFX>();
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _playerAttack = GetComponent<InitialPlayerAttack>();

        _animDefaultSpeed = _anim.speed;

        _fadeIn = GameObject.FindGameObjectWithTag("Fade In").GetComponent<FadeVFX>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Ignorar Colisão caso o Player estiver fazendo um Ataque / Carregando
        if (_playerAttack.IsAttacking ||
            InitialPlayerStateMachine.StateManager.CompareState(InitialPlayerStateMachine.InitialPlayerStates.Loading))
            return;

        if (col.gameObject.layer == collisionLayers.EnemyLayer)
        {
            // Aplicando Dano
            ChangeCurrentHealth(-col.gameObject.GetComponent<Enemy>().Damage);

            if (col.gameObject.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour behaviourScript))
            {
                if (!behaviourScript.isRange) behaviourScript.Anim.SetTrigger("Attacking");
            }

            // Aplicando Knockback
            if (_spr.flipX) ApplyKnockback(Vector2.right);
            else ApplyKnockback(Vector2.left);

            // Aplicando Invencibilidade temporária
            StartCoroutine(SetInvencibilityInterval(invencibilityTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.SpikeLayer && !_playerAttack.IsAttacking)
            ChangeCurrentHealth(-maxHealth);
        else if (col.gameObject.layer == collisionLayers.ArmorTriggerLayer)
            _audioManager.PlaySFX("nave terminal");
    }

    public void ChangeCurrentHealth(int points)
    {
        currentHealth = Mathf.Clamp(currentHealth + points, 0, maxHealth);
        _playerHealthBar.SetHealthBar(currentHealth);

        if (currentHealth == 0 && !_fadeIn.enabled)
        {
            InitialPlayerStateMachine.StateManager.SetState(InitialPlayerStateMachine.InitialPlayerStates.Dead);

            _playerAttack.enabled = false;
            GetComponent<InitialPlayerMovement>().enabled = false;
            _anim.SetBool("isGrounded", true);
            _rb.velocity = Vector2.zero;
            _anim.Play("Initial Player Death Animation");

            var alpha = _spr.color;
            alpha.a *= 0.75f;
            _spr.color = alpha;

            //_fadeIn.enabled = true;

            gameOverCanva.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            _audioManager.PlayMusic("game over");


            _audioManager.PlayMusic("game over");

            //StartCoroutine(RestartScene(restartSceneTime));
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    private IEnumerator SetInvencibilityInterval(float time)
    {
        _blinkVFX.SetBlink();
        // Desabilito a colisão entre o player e os inimigos
        Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.EnemyLayer, true);
        yield return new WaitForSeconds(time);
        // Habilito novamente a colisão entre o player e os inimigos
        Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.EnemyLayer, false);
    }

    public void ApplyKnockback(Vector2 dir)
    {
        _anim.speed = 0;
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        _rb.AddForce(dir * knockbackForce * Time.deltaTime, ForceMode2D.Impulse);
        InitialPlayerStateMachine.StateManager.SetState(InitialPlayerStateMachine.InitialPlayerStates.Damaged);
        StartCoroutine(StopKnockback(knobackTime));
    }

    private IEnumerator StopKnockback(float t)
    {
        yield return new WaitForSeconds(t);
        InitialPlayerStateMachine.StateManager.SetState(InitialPlayerStateMachine.InitialPlayerStates.Idle);
        _anim.speed = _animDefaultSpeed;
    }

    private IEnumerator RestartScene(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
