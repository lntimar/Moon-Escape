using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private float knockbackForce;
    [SerializeField] private GameObject[] effects;

    [Header("Collision Layers:")]
    [SerializeField] CollisionLayers collisionLayers;

    // Components
    private Enemy _enemy;
    private EnemyBehavior _enemyBehavior;
    private BlinkSpriteVFX _blink;
    private Rigidbody2D _rb;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _enemyBehavior = GetComponent<EnemyBehavior>();
        _blink = GetComponent<BlinkSpriteVFX>();
        _rb = GetComponent<Rigidbody2D>();

        ChangeCurrentHealth(_enemy.MaxHealth);
    }

    public void ChangeCurrentHealth(int points)
    {
        currentHealth = Mathf.Clamp(currentHealth + points, 0, _enemy.MaxHealth);

        if (currentHealth == 0)
        {
            // Inimigo Morre
            Destroy(gameObject);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ApplyEffect(BananaType.Types effect)
    {
        switch (effect)
        {
            case BananaType.Types.Ice:
                effects[0].GetComponent<IceEffect>().Freeze();
                break;

            case BananaType.Types.Bomb:
                effects[1].GetComponent<BombEffect>().Explode();
                break;

            case BananaType.Types.Eletric:
                effects[2].GetComponent<EletricEffect>().Eletrify();
                break;
        }
        _blink.SetBlink();
    }

    public void ApplyKnockback(Vector2 direction)
    {
        StopAllCoroutines();
        _enemyBehavior.SetMovement(false);
        _rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(ResetMovement(0.1f));
    }

    private IEnumerator ResetMovement(float t)
    {
        yield return new WaitForSeconds(t);
        _enemyBehavior.SetMovement(true);
    }
}
