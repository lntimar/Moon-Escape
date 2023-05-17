using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIceEffect : MonoBehaviour
{
    [Header("Enemy References:")]
    [SerializeField] private EnemyCollision enemyColScript;
    [SerializeField] private EnemyBehaviour enemyBehaviourScript;
    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] private SpriteRenderer enemySpr;

    [Header("Ice Banana:")]
    [SerializeField] private Banana iceBanana;
    [SerializeField] private float freezeTime;

    [Header("Collision Layers:")]
    [SerializeField] private CollisionLayers collisionLayers;

    // Components
    private SpriteRenderer _spr;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
    }

    public void Freeze()
    {
        enemyColScript.gameObject.layer = collisionLayers.IgnoreIceLayer;
        enemyBehaviourScript.enabled = false;

        _spr.enabled = true;
        _spr.sprite = enemySpr.sprite;
        _spr.flipX = enemySpr.flipX;
        enemySpr.enabled = false;
        
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
        enemySpr.enabled = true;

        enemyColScript.gameObject.layer = collisionLayers.EnemyLayer;
        enemyBehaviourScript.enabled = true;
        enemyBehaviourScript.StartCoroutine(enemyBehaviourScript.EnemyWaits());
    }
}
