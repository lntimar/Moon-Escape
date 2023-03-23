using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Por enquanto... Insira os valores no Inspector, dps todos serão retirados no banco de dados
    public int MaxHealth;
    public int Damage;
    public int Speed;
    public string Item;

    /* Informações que irão ser retiradas do banco de dados
    [HideInInspector] public int MaxHealth { get; private set; }
    [HideInInspector] public int Damage { get; private set; }
    [HideInInspector] public float Speed { get; private set; }
    [HideInInspector] public string Item {get; private set; }
     */

    // TODO: Integração entre o SQL e C#
}
