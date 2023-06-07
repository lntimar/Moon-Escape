using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    // Components
    private SpriteRenderer _spr;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();

        _spr.sprite = sprites[Random.Range(0, sprites.Length)];

        StartCoroutine(ChangeSprite(10f));
    }

    private IEnumerator ChangeSprite(float t)
    {
        yield return new WaitForSeconds(t);
        var newSprite = sprites[Random.Range(0, sprites.Length)];
        while (newSprite == _spr.sprite)
        {
            newSprite = sprites[Random.Range(0, sprites.Length)];
        }
        _spr.sprite = newSprite;

        StartCoroutine(ChangeSprite(10f));
    }
}
