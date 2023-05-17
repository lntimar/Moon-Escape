using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBombEffect : MonoBehaviour
{
    [Header("Boss References:")]
    [SerializeField] private BossCollision bossColScript;

    [Header("Bomb Banana:")]
    [SerializeField] private Banana bombBanana;

    [Header("Collision Layers:")]
    [SerializeField] CollisionLayers collisionLayers;

    [Header("Stop Explosion:")]
    [SerializeField] private float explosionTime;
    [SerializeField] private float cooldownTime;

    // Components
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Explode()
    {
        bossColScript.gameObject.layer = collisionLayers.IgnoreExplosionLayer;
        _anim.SetTrigger("explode");

        StartCoroutine(StopExplosion(explosionTime));
        StartCoroutine(SetCooldown(cooldownTime));
    }

    private IEnumerator StopExplosion(float t)
    {
        yield return new WaitForSeconds(t);
    }

    private IEnumerator SetCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        bossColScript.gameObject.layer = collisionLayers.BossLayer;
    }
}
