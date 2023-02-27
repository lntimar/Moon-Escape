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
    [SerializeField] private int defaultBananaIndex;
    [SerializeField] private int currentBananaIndex;
    [SerializeField] private int maxEnergy; // Provavelmente vai ser static
    [SerializeField] private int currentEnergy; // Provavelmente vai ser static
    [SerializeField] private BananaType[] bananas = new BananaType[4]; // Lista dos tipos de bananas que possuo no momento

    // Direção do Disparo
    private Vector2 _direction;

    private void Start()
    {
        // Colocando os Valores Iniciais
        currentBananaIndex = defaultBananaIndex;
        ChangeCurrentEnergy(maxEnergy);
    }

    private void Update()
    {
        if (PlayerStateMachine.StateManager.IsNotFine() == true) return;
        
        ShootInput();
        ChangeInput();
    }

    // Disparo
    private void ShootInput()
    {
        if (canShoot && Input.GetButton("Shoot") && currentEnergy - bananas[currentBananaIndex].GetEnergyCost() >= 0)
        {
            canShoot = false;
            SpawnBanana(bananas[currentBananaIndex].GetComponent<BananaMovement>());
            ChangeCurrentEnergy(bananas[currentBananaIndex].GetEnergyCost());
            SetShootInterval(shootInterval);
        }

        if (Input.GetButtonUp("Shoot"))
        {
            StopAllCoroutines(); // Pare todas as coroutines SetShootInterval
            canShoot = true;
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
        if (Input.GetButtonDown("Change Banana")) // Troca para a seguinte
        {
            if (currentBananaIndex + 1 >= bananas.Length)
            {
                currentBananaIndex = defaultBananaIndex;
            }
            else if (bananas[currentBananaIndex + 1] != null)
            {
                currentBananaIndex++;
            }
        }
        else if (Input.GetButtonDown("Change Default Banana")) // Troca para a default
        {
            currentBananaIndex = defaultBananaIndex;
        }
    }

    public void AddBananaType(BananaType b)
    {
        for (int i = 0; i < bananas.Length; i++)
        {
            if (bananas[i] == null)
            {
                bananas[i] = b;
                break;
            }
        }
    }
}
