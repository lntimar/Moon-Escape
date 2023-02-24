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
    [SerializeField] private BananaType.Types defaultType;
    [SerializeField, ReadOnly] private BananaType.Types currentType;
    [SerializeField] private float defaultEnergy;
    [SerializeField] private float currentEnergy;
    [SerializeField] private BananaType[] bananas;

    private Vector2 _direction;

    private void Start()
    {
        currentType = defaultType;
        currentEnergy = defaultEnergy;
    }

    private void Update()
    {
        ShootInput();
    }

    private void ShootInput()
    {
        if (canShoot && Input.GetButton("Shoot"))
        {
            canShoot = false;
            SpawnBanana();
            SetShootInterval(shootInterval);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        _direction = dir;
    }

    private void SpawnBanana()
    {
        foreach (BananaType b in bananas)
        {
            if (b.GetType() == currentType)
            {
                var banana = Instantiate(b, transform.position, Quaternion.identity);
                banana.GetComponent<BananaMovement>().SetDirection(_direction);
                break;
            }
        }
    }

    private IEnumerator SetShootInterval(float time)
    {
        yield return new WaitForSeconds(time);
        canShoot = true;
    }
}
