using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkSpriteVFX : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private float interval;

    // Components
    private SpriteRenderer _spr;

    private bool _isBlinking;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
    }

    public void SetBlink()
    {
        _isBlinking = true;
        StartCoroutine(ApplyBlink(interval));
        StartCoroutine(SetBlinkTime(time));
    }

    private IEnumerator SetBlinkTime(float time)
    {
        yield return new WaitForSeconds(time);
        _isBlinking = false;
        _spr.enabled = true;
    }

    private IEnumerator ApplyBlink(float time)
    {
        _spr.enabled = !_spr.enabled;
        yield return new WaitForSeconds(time);
        if (_isBlinking) StartCoroutine(ApplyBlink(interval));
    }
}
