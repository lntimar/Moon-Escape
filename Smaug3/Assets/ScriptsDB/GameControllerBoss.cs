using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerBoss : MonoBehaviour
{
    // Insira o id desejado no Inspector da Unity
    [SerializeField] private int bossId;

    // Todas as possiveis bananas especiais no jogo
    [SerializeField] private BananaType[] bananasSpecial;

    // Instância
    private BossModel _bossInstance;

    // Components
    private Boss _boss;

    private void Awake()
    {
        // Pegando o Script do Boss no Game
        _boss = GetComponent<Boss>();

        // Acessando a instância no BD com base no Id desejado
        _bossInstance = GamesCodeDataSource.Instance.BossDAO.GetBoss(bossId);

        // Atribuindo os valores
        _boss.MaxHealth = _bossInstance.MaxHealth;
        _boss.Speed = _bossInstance.MoveSpeed;
        _boss.Damage = _bossInstance.Damage;
        _boss.Name = _bossInstance.Name;

        // Selecionando a Banana no Jogo com base no BananaId armazenado pela instância no Boss
        switch (_bossInstance.BananaId) 
        {
            // BananaId == 2 -> Banana de Gelo
            case 2:
                _boss.BananaType = bananasSpecial[0];
                break;

            // BananaId == 3 -> Banana Bomba
            case 3:
                _boss.BananaType = bananasSpecial[1];
                break;

            // BananaId == 4 -> Banana Elétrica
            case 4:
                _boss.BananaType = bananasSpecial[2];
                break;
        }
    }
}
