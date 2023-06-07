using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMovement : MonoBehaviour
{
    // Components
    private Banana _banana;
    private Rigidbody2D _rb;
    private SpriteRenderer _spr;

    // Direção do Movimento 
    private Vector2 _direction;

    // Efeito
    [SerializeField] private Color effectColor;
    [SerializeField] private GameObject bananaEffectPrefab;


    private void Start()
    {
        _banana = GetComponent<Banana>();
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();

        StartCoroutine(ApplyEffect(0.01f));

        if (_direction.x < 0f) _spr.flipX = true;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    public void SetDirection(Vector2 dir)
    {
        _direction = dir;
    }

    public Vector2 GetDirection()
    {
        return _direction;
    }

    private void ApplyMovement()
    {
        _rb.velocity = _direction * _banana.Speed;
    }

    private IEnumerator ApplyEffect(float t)
    {
        yield return new WaitForSeconds(t);
        var effect = Instantiate(bananaEffectPrefab, transform.position, Quaternion.identity);
        var effectSpr = effect.GetComponent<SpriteRenderer>();
        effectSpr.sprite = GetComponent<SpriteRenderer>().sprite;
        effectSpr.color = effectColor;
        StartCoroutine(ApplyEffect(0.01f));
    }
}
