using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    // Components
    private Image _img;

    private void Awake()
    {
        _img = GetComponent<Image>();
    }

    public void SetHealthBar(int health)
    {
        int currentHealth = health;
        _img.sprite = sprites[currentHealth]; // Qualquer sprite de vida acima de 0
    }
}
