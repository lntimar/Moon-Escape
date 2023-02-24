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
        // Examples
        Normal,
        Fire,
        Ice,
        Poison
    }

    public void SetType(string name)
    {
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

    // TODO: Efeitos Bananas
}
