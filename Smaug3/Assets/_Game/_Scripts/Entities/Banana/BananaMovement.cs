using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMovement : MonoBehaviour
{
    // Components
    private Banana _banana;
    private Rigidbody2D _rb;

    // Direção do Movimento 
    private Vector2 _direction;

    private void Start()
    {
        _banana = GetComponent<Banana>();
        _rb = GetComponent<Rigidbody2D>();
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
}
