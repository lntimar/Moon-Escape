using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    // Unity Access Fields
    [SerializeField] private FadeType type;
    [SerializeField] private float speed;

    private enum FadeType
    {
        FadeOut,
        FadeIn
    }

    // Components
    private SpriteRenderer _spr;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ApplyFade();
    }

    private void ApplyFade()
    {
        var color = _spr.color;
        var alpha = color.a;

        if (type == FadeType.FadeOut)
        {
            if (alpha > 0.0f)
            {
                alpha -= speed * Time.deltaTime;
                color.a = alpha;
                _spr.color = color;
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
                _spr.color = color;
            }
            else
            {
                this.enabled = false;
            }
        }
    }
}
