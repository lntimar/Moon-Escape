using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private float minBonusHealth;
    [SerializeField] private float minBonusEnergy;
    [SerializeField] private Item itemLifePrefab;
    [SerializeField] private Item itemEnergyPrefab;

    public void SpawnBonus(bool hasEnergy)
    {
        if (hasEnergy)
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
            int currentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollision>().GetCurrentHealth();

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
