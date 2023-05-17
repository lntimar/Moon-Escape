using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIceEffect : MonoBehaviour
{
    [Header("Boss References:")]
    [SerializeField] private Boss bossScript;
    [SerializeField] private BossCollision bossColScript;
    [SerializeField] private Rigidbody2D bossRb;
    [SerializeField] private SpriteRenderer bossSpr;
    [SerializeField] private Animator bossAnim;

    [Header("Ice Banana:")]
    [SerializeField] private Banana iceBanana;
    [SerializeField] private float freezeTime;

    [Header("Collision Layers:")]
    [SerializeField] private CollisionLayers collisionLayers;

    // References
    private MonoBehaviour _bossBehaviourScript;

    // Components
    private SpriteRenderer _spr;

    // Stop Animator
    private float _animDefaultSpeed;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
        _animDefaultSpeed = bossAnim.speed;
    }

    public void Freeze()
    {
        _bossBehaviourScript = transform.parent.GetComponent<BossCollision>().BehaviourScript;
        bossColScript.gameObject.layer = collisionLayers.IgnoreIceLayer;
        _bossBehaviourScript.enabled = false;
        bossAnim.speed = 0f;
        bossRb.velocity = Vector2.zero;

        _spr.enabled = true;
        _spr.flipX = bossSpr.flipX;
        
        _spr.sprite = bossSpr.sprite;

        bossSpr.enabled = false;
        StartCoroutine(SetFreezeInterval(freezeTime));
    }

    private IEnumerator SetFreezeInterval(float t)
    {
        yield return new WaitForSeconds(t);
        StopFreeze();
    }

    private void StopFreeze()
    {
        _spr.enabled = false;

        bossColScript.gameObject.layer = collisionLayers.BossLayer;
        _bossBehaviourScript.enabled = true;
        bossAnim.speed = _animDefaultSpeed;
        bossSpr.enabled = true;

        if (bossScript.Name == "FinalBoss") transform.parent.gameObject.GetComponent<FinalBossBehaviour>().EnableBoss();
    }
}
