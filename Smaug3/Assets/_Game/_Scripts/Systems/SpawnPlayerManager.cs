using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Awake()
    {
        var playerCount = GameObject.FindGameObjectsWithTag("Player");
        if (playerCount.Length == 0) player.SetActive(true);
    }
}
