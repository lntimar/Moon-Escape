using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMovement : MonoBehaviour
{
    private Vector2 _direction;

    // Components
    private Banana _banana;
    private Rigidbody2D _rb;

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

    private void ApplyMovement()
    {
        _rb.velocity = _direction * _banana.Speed;
    }
}
