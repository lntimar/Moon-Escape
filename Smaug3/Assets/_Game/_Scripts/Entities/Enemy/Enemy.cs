using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Por enquanto... Insira os valores no Inspector, dps todos ser�o retirados no banco de dados
    public int MaxHealth;
    public int Damage;
    public float Speed;

    /* Informa��es que ir�o ser retiradas do banco de dados
    [HideInInspector] public int Life { get; private set; }
    [HideInInspector] public int Damage { get; private set; }
    [HideInInspector] public float Speed { get; private set; }
    */

    // TODO: Integra��o entre o SQL e C#
}
