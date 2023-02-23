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
    [SerializeField] private string defaultType;
    [SerializeField, ReadOnly] private string currentType;
    [SerializeField] private float defaultEnergy;
    [SerializeField] private float currentEnergy;
    [SerializeField] private Banana[] bananas;

    private Vector2 _direction;

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
        /*
        foreach (Banana b in bananas)
        {

        }
        */
    }

    private IEnumerator SetShootInterval(float time)
    {
        yield return new WaitForSeconds(time);
        canShoot = true;
    }
}
