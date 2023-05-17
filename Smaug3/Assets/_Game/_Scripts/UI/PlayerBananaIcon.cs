using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBananaIcon : MonoBehaviour
{
    [Header("Banana Icon:")]
    [SerializeField] private Sprite[] iconSprites;
    [SerializeField] private bool disableOnAwake = true;

    // Components
    private Image _img;

    private void Awake()
    {
        _img = GetComponent<Image>();

        if (disableOnAwake) gameObject.SetActive(false);
    }

    public void SetCurrentIcon(int index)
    {
        _img.sprite = iconSprites[index];
    }
}
