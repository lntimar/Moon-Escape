using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    [Header("Health:")]
    [SerializeField] private int maxHealth;
    [SerializeField, ReadOnly] private int currentHealth;

    [Header("Invencibility:")]
    [SerializeField, ReadOnly] private bool isInvincible = false;
    [SerializeField] private float invencibilityTime;

    private void Awake()
    { 
        SetHealth(maxHealth);
    }

    private void Update()
    {
        SetHealth(GetHealth() - 1);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (isInvincible) return; // Ignore caso estiver invencível
        
        // TODO: Verificação de Colisão entre inimigos, projetéis, armadilhas, etc...
    }

    private void ApplyDamage(int damage)
    {
        currentHealth -= damage;
    }

    private void SetHealth(int points)
    {
        currentHealth = Mathf.Clamp(points, 0, maxHealth);

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
        isInvincible = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
    }
}
