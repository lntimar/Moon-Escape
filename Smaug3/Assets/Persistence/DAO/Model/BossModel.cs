using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossModel : MonoBehaviour
{
    public int Id { get; set; }
    public int MaxHealth { get; set; }
    public int Damage { get; set; }
    public string Name { get; set; }
    public float MoveSpeed { get; set; }
    public int BananaId { get; set; }

    public BossModel() { }

    public BossModel(int id, int maxHealth, int damage, string name, float moveSpeed, int bananaId)
    {
        Id = id;
        MaxHealth = maxHealth;
        Damage = damage;
        Name = name;
        MoveSpeed = moveSpeed;
        BananaId = bananaId;
    }
}