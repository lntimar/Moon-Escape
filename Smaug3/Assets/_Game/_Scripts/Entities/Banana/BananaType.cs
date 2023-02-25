using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaType : MonoBehaviour
{
    private Types _type;
    private int _energyCost;

    private void Awake()
    {
        SetType(GetComponent<Banana>().Name);
        SetEnergyCost(GetComponent<Banana>().EnergyCost);
    }

    public enum Types
    {
        // Todos os Tipos de bananas
        Normal,
        Fire,
        Ice,
        Poison
    }

    private void SetType(string name)
    {
        // Defino o tipo com base no nome
        switch (name)
        {
            case "Fire":
                _type = Types.Fire;
                break;

            case "Ice":
                _type = Types.Ice;
                break;

            case "Poison":
                _type = Types.Poison;
                break;

            default:
                _type = Types.Normal;
                break;
        }
    }

    public Types GetType()
    {
        return _type;
    }

    private void SetEnergyCost(int cost)
    {
        _energyCost = cost;
    }

    public int GetEnergyCost()
    {
        return _energyCost;
    }

    // TODO: Efeitos Bananas
}
