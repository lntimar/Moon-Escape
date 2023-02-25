using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    // Informações que irão ser retiradas do banco de dados 
    [HideInInspector] public string Name { get { return Name; } private set { Name = value; } }
    [HideInInspector] public int Damage { get { return Damage; } private set { Damage = value; } }
    [HideInInspector] public float Speed { get { return Speed; } private set { Speed = value; } }
    [HideInInspector] public int EnergyCost {get {return  EnergyCost;} private set {Speed = value;} }

    // TODO: Integração entre o SQL e C#
}
