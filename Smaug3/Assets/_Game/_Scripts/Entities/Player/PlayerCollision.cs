using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    [Header("Health:")]
    [SerializeField] private int maxHealth; // Provavelmente vai ser static
    [SerializeField] private int currentHealth; // Provavelmente vai ser static
    [SerializeField] private SpriteRenderer playerDeathSpr;
    [SerializeField] private Animator playerDeathAnim;

    [Header("Invencibility:")]
    [SerializeField] private float invencibilityTime;

    [Header("Collision Layers:")]
    [SerializeField] CollisionLayers collisionLayers;

    [Header("Knockback:")] 
    [SerializeField] private float knockbackForce;

    // Components
    private BlinkSpriteVFX _blinkVFX;
    private SpriteRenderer _spr;
    private Rigidbody2D _rb;

    
    private void Awake()
    { 
        ChangeCurrentHealth(maxHealth);
    }

    private void Start()
    {
        _blinkVFX = GetComponent<BlinkSpriteVFX>();
        _spr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // TODO: Verificação de Colisão entre inimigos, projetéis, armadilhas, etc...
    }

    private void ChangeCurrentHealth(int points)
    {
        currentHealth = Mathf.Clamp(currentHealth + points, 0, maxHealth);

        if (currentHealth == 0)
        {
            PlayerStateMachine.StateManager.SetState(PlayerStateMachine.PlayerStates.Dead);
            _spr.enabled = false;
            playerDeathSpr.enabled = true;

            if (_spr.flipX) playerDeathAnim.SetTrigger("deadLeft");
            else playerDeathAnim.SetTrigger("deadRight");

            //SceneManager.LoadScene(SceneManager.GetActiveScene().name); // TODO: Remover dps
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    } 

    private IEnumerator SetInvencibilityInterval(float time)
    {
        // Desabilito a colisão entre o player e os inimigos
        Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.EnemyLayer, true);
        yield return new WaitForSeconds(time);
        // Habilito novamente a colisão entre o player e os inimigos
        Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.EnemyLayer, false);
    }

    public void ApplyKnockback(Vector2 dir)
    {
        _rb.AddForce(dir * knockbackForce * Time.deltaTime, ForceMode2D.Impulse);
    }
}
