using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    // Informa��es que ir�o ser retiradas do banco de dados 
    [HideInInspector] public string Name { get; private set; }
    [HideInInspector] public int Damage { get; private set; }
    [HideInInspector] public float Speed { get; private set; }
    [HideInInspector] public int EnergyCost {get; private set; }

    // TODO: Integra��o entre o SQL e C#
}
