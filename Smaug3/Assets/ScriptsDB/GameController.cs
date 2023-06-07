using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Persistence.DAO.Implementation;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using System;
public class GameController : MonoBehaviour
{
    void Start()
    {
        Debug.LogWarning("Bananas Cadastradas");
        // Mostrando todas as bananas cadastradas
        for (int i = 1; i < 5; i++)
        {
            var w = GamesCodeDataSource.Instance.BananaDAO.GetBanana(i);
        }


        Debug.LogWarning("Bosses Cadastrados");
        // Mostrando todos os bosses cadastrados
        for (int i = 1; i < 6; i++)
        {
            var x = GamesCodeDataSource.Instance.BossDAO.GetBoss(i);
        }
    }
}