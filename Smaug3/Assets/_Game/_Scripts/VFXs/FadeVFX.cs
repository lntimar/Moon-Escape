using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeVFX : MonoBehaviour
{
    // Unity Access Fields
    [SerializeField] private FadeType type;
    [SerializeField] private float speed;
    [SerializeField] private bool isUI;

    private enum FadeType
    {
        FadeOut,
        FadeIn
    }

    // Components
    private SpriteRenderer _spr;
    private Image _img;

    private void Start()
    {
        if (isUI) _img = GetComponent<Image>();
        else _spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ApplyFade();
    }

    private void ApplyFade()
    {
        Color color;
        if (isUI) color = _img.color;
        else color = _spr.color;

        var alpha = color.a;

        if (type == FadeType.FadeOut)
        {
            if (alpha > 0.0f)
            {
                alpha -= speed * Time.deltaTime;
                color.a = alpha;
                if (isUI) _img.color = color;
                else _spr.color = color;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (alpha < 1.0f)
            {
                alpha += speed * Time.deltaTime;
                color.a = alpha;
                if (isUI) _img.color = color;
                else _spr.color = color;
            }
            else
            {
                this.enabled = false;
            }
        }
    }
}
