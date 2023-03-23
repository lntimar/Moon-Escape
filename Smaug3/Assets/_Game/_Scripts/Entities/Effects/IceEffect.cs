using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffect : MonoBehaviour
{
    [Header("Enemy References:")]
    [SerializeField] private EnemyCollision enemyColScript;
    [SerializeField] private EnemyBehavior enemyBehaviorScript;
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
        enemyBehaviorScript.SetMovement(false);
        enemyRb.velocity = Vector2.zero;

        _spr.enabled = true;
        _spr.sprite = enemySpr.sprite;

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

        enemyColScript.gameObject.layer = collisionLayers.EnemyLayer;
        enemyBehaviorScript.SetMovement(true);
    }
}
