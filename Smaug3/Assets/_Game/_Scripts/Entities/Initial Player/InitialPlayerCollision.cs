using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InitialPlayerCollision : MonoBehaviour
{
    [Header("Health:")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [Header("Invencibility")] 
    [SerializeField] private float invencibilityTime;

    [Header("Collision Layers:")]
    [SerializeField] CollisionLayers collisionLayers;

    // Components
    private BlinkSpriteVFX _blinkVFX;

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
            InitialPlayerStateMachine.StateManager.SetState(InitialPlayerStateMachine.InitialPlayerStates.Dead);
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
        Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.EnemyLayer, true);
        yield return new WaitForSeconds(time);
        // Habilito novamente a colisão entre o player e os inimigos
        Physics2D.IgnoreLayerCollision(collisionLayers.PlayerLayer, collisionLayers.EnemyLayer, false);
    }
}
