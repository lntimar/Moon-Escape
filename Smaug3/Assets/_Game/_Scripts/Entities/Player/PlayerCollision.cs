using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    [Header("Health:")]
    [SerializeField] private int maxHealth; // Provavelmente vai ser static
    [SerializeField, ReadOnly] private int currentHealth; // Provavelmente vai ser static

    [Header("Invencibility:")]
    [SerializeField] private float invencibilityTime;

    // References
    private BlinkSpriteVFX _blinkVFX;

    // Collision Layers
    private int _playerColLayer = 3;
    private int _enemyColLayer = 6;

    private void Awake()
    { 
        ChangeCurrentHealth(maxHealth);
    }

    private void Start()
    {
        _blinkVFX = GetComponent<BlinkSpriteVFX>();
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // TODO: Remover dps
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    } 

    private IEnumerator SetInvencibilityInterval(float time)
    {
        // Desabilito a colisão entre o player e os inimigos
        Physics2D.IgnoreLayerCollision(_playerColLayer, _enemyColLayer, true);
        yield return new WaitForSeconds(time);
        // Habilito novamente a colisão entre o player e os inimigos
        Physics2D.IgnoreLayerCollision(_playerColLayer, _enemyColLayer, false);
    }
}
