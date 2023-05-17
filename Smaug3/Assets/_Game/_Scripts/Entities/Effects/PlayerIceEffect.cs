using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIceEffect : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] private Color iceColor;
    [SerializeField] private float freezeTime;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float dragMultiplier;

    // Components
    private SpriteRenderer _spr;
    private Rigidbody2D _rb;

    private Color _sprColor;
    private float _rbGravityScale;
    private float _rbDrag;

    [HideInInspector] public bool IsFreeze = false;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();

        _sprColor = _spr.color;
        _rbGravityScale = _rb.gravityScale;
        _rbDrag = _rb.drag;
    }

    public void Freeze()
    {
        IsFreeze = true;

        _rb.gravityScale *= gravityMultiplier;
        _rb.drag *= dragMultiplier;
        _spr.color = iceColor;

        StartCoroutine(StopFreeze(freezeTime));
    }

    private IEnumerator StopFreeze(float t)
    {
        yield return new WaitForSeconds(t);
        _rb.gravityScale = _rbGravityScale;
        _rb.drag = _rbDrag;
        _spr.color = _sprColor;

        IsFreeze = false;
    }
}
