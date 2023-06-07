using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaModel : MonoBehaviour
{
    public int Id { get; set; }
    public int Damage { get; set; }
    public string Name { get; set; }
    public int EnergyCost { get; set; }
    public float MoveSpeed { get; set; }

    public BananaModel() { }

    public BananaModel(int id, int damage, string name, int energyCost, float moveSpeed)
    {
        Id = id;
        Damage = damage;
        Name = name;
        EnergyCost = energyCost;
        MoveSpeed = moveSpeed;
    }
}