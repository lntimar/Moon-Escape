using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    // Por enquanto... Insira os valores no Inspector, dps todos serão retirados no banco de dados
    public string Name;
    public int Damage;
    public float Speed;
    public int EnergyCost;
    public float LifeTime;

    /* Informações que irão ser retiradas do banco de dados 
    [HideInInspector] public string Name { get; private set; }
    [HideInInspector] public int Damage { get; private set; }
    [HideInInspector] public float Speed { get; private set; }
    [HideInInspector] public int EnergyCost {get; private set; }
    */

    // TODO: Integração entre o SQL e C#
}
