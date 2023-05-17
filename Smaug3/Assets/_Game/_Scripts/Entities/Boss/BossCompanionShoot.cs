using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCompanionShoot : MonoBehaviour
{
    [Header("Spawn:")]
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private Transform spawnPoint;

    [Header("Prefab:")]
    [SerializeField] private BossCompanionOrb orbPrefab; 

    private void Start()
    {
        StartCoroutine(StartSpawnOrbInterval());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator StartSpawnOrbInterval()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        Instantiate(orbPrefab, spawnPoint.position, Quaternion.identity);

        StartCoroutine(StartSpawnOrbInterval());
    }
}
