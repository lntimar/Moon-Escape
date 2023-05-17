using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaType : MonoBehaviour
{
    private Types _type;

    private void Awake()
    {
        SetType(GetComponent<Banana>().Name);
    }

    public enum Types
    {
        // Todos os Tipos de bananas
        Default,
        Ice,
        Bomb,
        Eletric
    }

    private void SetType(string name)
    {
        // Defino o tipo com base no nome
        switch (name)
        {
            case "Ice":
                _type = Types.Ice;
                break;

            case "Bomb":
                _type = Types.Bomb;
                break;

            case "Eletric":
                _type = Types.Eletric;
                break;

            default:
                _type = Types.Default;
                break;
        }
    }

    public Types GetBananaType()
    {
        return _type;
    }

    public int GetEnergyCost()
    {
        return GetComponent<Banana>().EnergyCost;
    }
}
