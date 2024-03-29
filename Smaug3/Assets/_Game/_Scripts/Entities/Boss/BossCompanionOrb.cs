using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCompanionOrb : MonoBehaviour
{
    // Components
    private Rigidbody2D _rb;
    private Enemy _enemy;

    // Movement
    private Vector2 moveDir;

    // Efeito
    [SerializeField] private Color effectColor;
    [SerializeField] private GameObject orbEffectPrefab;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<Enemy>();

        SetMoveDirection();

        StartCoroutine(ApplyEffect(0.02f));
    }

    private void FixedUpdate()
    {
        ApplyMove();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }

    private void SetMoveDirection()
    {
        var playerTransf = GameObject.FindGameObjectWithTag("Player").transform;

        moveDir = (Vector2)(playerTransf.position - transform.position).normalized;
    }

    private void ApplyMove()
    {
        _rb.velocity = moveDir * _enemy.Speed;
    }

    private IEnumerator ApplyEffect(float t)
    {
        yield return new WaitForSeconds(t);
        var effect = Instantiate(orbEffectPrefab, transform.position, Quaternion.identity);
        var effectSpr = effect.GetComponent<SpriteRenderer>();
        effectSpr.sprite = GetComponent<SpriteRenderer>().sprite;
        effectSpr.color = effectColor;
        StartCoroutine(ApplyEffect(0.02f));
    }
}
