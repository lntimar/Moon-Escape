using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyBar : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    // Components
    private Image _img;

    private void Awake()
    {
        _img = GetComponent<Image>();
    }

    public void SetEnergyBar(int energy)
    {
        int currentEnergy = energy;
        
        _img.sprite = sprites[currentEnergy]; // Qualquer sprite de energia acima de 0
    }
}
