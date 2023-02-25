using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shoot:")]
    [SerializeField] private float shootInterval;
    [SerializeField, ReadOnly] private bool canShoot = true;

    [Header("Types:")] 
    [SerializeField] private BananaType defaultBanana;
    [SerializeField, ReadOnly] private BananaType.Types currentType;
    [SerializeField] private int maxEnergy; // Provavelmente vai ser static
    [SerializeField] private int currentEnergy; // Provavelmente vai ser static
    [SerializeField] private List<BananaType> bananas; // Lista dos tipos de bananas que possuo no momento

    // Direção do Disparo
    private Vector2 _direction;

    private void Start()
    {
        // Colocando os Valores Iniciais
        currentType = defaultBanana.GetType();
        ChangeCurrentEnergy(maxEnergy);
    }

    private void Update()
    {
        if (PlayerStateMachine.StateManager.IsFine() == false) return;
        
        ShootInput();
        ChangeInput();
    }

    // Disparo
    private void ShootInput()
    {
        if (canShoot && Input.GetButton("Shoot") && currentEnergy - GetCurrentBananaType().GetEnergyCost() >= 0)
        {
            canShoot = false;
            SpawnBanana(GetCurrentBananaType().GetComponent<BananaMovement>());
            ChangeCurrentEnergy(GetCurrentBananaType().GetEnergyCost());
            SetShootInterval(shootInterval);
        }
    }
    private IEnumerator SetShootInterval(float time)
    {
        yield return new WaitForSeconds(time);
        canShoot = true;
    }

    public void SetDirection(Vector2 dir)
    {
        _direction = dir;
    }

    // Banana
    private void SpawnBanana(BananaMovement b)
    {
        var banana = Instantiate(b, transform.position, Quaternion.identity);
        banana.SetDirection(_direction);
    }

    private BananaType GetCurrentBananaType()
    {
        var type = bananas[0];

        foreach (BananaType b in bananas)
        {
            if (b.GetType() == currentType)
            {
                type = b;
                break;
            }
        }

        return type;
    }

    // Energia
    public void ChangeMaxEnergy(int increment)
    {
        maxEnergy += increment;
    }

    public void ChangeCurrentEnergy(int increment)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + increment, 0, maxEnergy);
    }

    // Troca de tipo
    private void ChangeInput()
    {
        if (Input.GetButtonDown("Change Bullet"))
        {
            
        }
    }

    public void AddBananaType(BananaType b)
    {
        bananas.Add(b);
    }
}
